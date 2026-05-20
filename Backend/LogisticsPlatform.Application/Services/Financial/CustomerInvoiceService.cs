using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

namespace LogisticsPlatform.Application.Services.Financial
{
    public class CustomerInvoiceService : ICustomerInvoiceService
    {
        private readonly ICustomerInvoiceRepository _repo;
        private readonly ILoadRepository _loads;
        private readonly IOrderRepository _orders;
        private readonly IPermissionService _permission;
        private readonly INotificationService _notifications;

        public CustomerInvoiceService(
            ICustomerInvoiceRepository repo,
            ILoadRepository loads,
            IOrderRepository orders,
            IPermissionService permission,
            INotificationService notifications)
        {
            _repo = repo;
            _loads = loads;
            _orders = orders;
            _permission = permission;
            _notifications = notifications;
        }

        public async Task<CustomerInvoiceDto> GetAsync(Guid loadId)
        {
            var existing = await _repo.GetByLoadIdAsync(loadId);
            if (existing == null)
                throw new NotFoundException("Invoice not found.");

            return Map(existing);
        }

        public async Task<List<CustomerInvoiceDto>> ListAsync()
        {
            var invoices = await _repo.ListAsync();
            return invoices.Select(Map).ToList();
        }

        /// <summary>
        /// Manual create – nëse UI ka buton "Create Invoice" ose për skenarë specialë.
        /// Përndryshe, GetAsync tashmë e krijon vet-automatikisht.
        /// </summary>
        public async Task<CustomerInvoiceDto> CreateAsync(
            Guid loadId,
            CreateInvoiceDto dto,
            Guid userId)
        {
            var existing = await _repo.GetByLoadIdAsync(loadId);
            if (existing != null)
                throw new BusinessValidationException("An invoice already exists for this load.");

            var load = await _loads.GetByIdAsync(loadId)
                ?? throw new NotFoundException("Load not found.");

            if (load.CustomerId == null)
                throw new BusinessValidationException("Cannot create customer invoice: load has no assigned customer.");

            var invoice = await CreateInternalAsync(load, dto, userId, isAuto: false);

            return Map(invoice);
        }

        /// <summary>
        /// Internal helper që bën realisht krijimin e invoice (qoftë auto, qoftë manual).
        /// </summary>
        private async Task<CustomerInvoice> CreateInternalAsync(
        Load load,
        CreateInvoiceDto dto,
        Guid userId,
        bool isAuto)
        {
            var lineItems = BuildLineItemsFromLoad(load);

            if (!lineItems.Any())
                throw new BusinessValidationException("Cannot create invoice: no billable items.");

            var total = lineItems.Sum(x => x.Amount);

            var invoice = new CustomerInvoice
            {
                LoadId = load.Id,
                Load = load,
                CustomerId = load.CustomerId,
                Customer = load.Customer!,

                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}",
                InvoiceDate = dto.InvoiceDate,
                DueDate = dto.DueDate,
                InvoiceType = InvoiceType.Customer,
                Status = InvoiceStatus.Draft,

                Notes = dto.Notes,
                LineItems = lineItems,
                TotalAmount = total,

                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(invoice);
            await _repo.SaveChangesAsync();

            return invoice;
        }


        /// <summary>
        /// Ndërton line items për invoice nga:
        /// - Load.CustomerRate (Freight Charge)
        /// - Load.Cost.LineItems me IsCustomer = true (accessorials për customer)
        /// </summary>
        private static List<CustomerInvoiceLineItem> BuildLineItemsFromLoad(Load load)
        {
            var result = new List<CustomerInvoiceLineItem>();

            // 1) Freight line – CustomerRate
            var customerRate = load.CustomerRate ?? 0;
            if (customerRate > 0)
            {
                result.Add(new CustomerInvoiceLineItem
                {
                    Description = "Freight Charge",
                    Qty = 1,
                    Price = customerRate,
                    Amount = customerRate,
                    Billable = true
                });
            }

            // 2) Extra billable line items (accessorials) – nga LoadCost ku IsCustomer = true
            if (load.Cost?.LineItems != null)
            {
                var extras = load.Cost.LineItems
                    .Where(x => x.IsCustomer)
                    .Select(x => new CustomerInvoiceLineItem
                    {
                        Description = x.Notes ?? x.Type.ToString(),
                        Qty = x.Qty,
                        Price = x.Price,
                        Amount = x.Qty * x.Price,
                        Billable = true,
                        Notes = x.Notes
                    });

                result.AddRange(extras);
            }

            return result;
        }

        public async Task<CustomerInvoice> GetByIdAsync(Guid invoiceId)
        {
            var invoice = await _repo.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new NotFoundException("Invoice not found.");

            return invoice;
        }

        public async Task UpdateStatusAsync(Guid invoiceId, InvoiceStatus status, Guid userId)
        {
            if (!await _permission.HasPermissionAsync(userId, Permission.Financial_Invoice_UpdateStatus))
                throw new ForbiddenException("You are not allowed to update invoice status.");

            var invoice = await _repo.GetByIdAsync(invoiceId)
                ?? throw new NotFoundException("Invoice not found.");

            invoice.Status = status;
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedByUserId = userId;

            await SyncOrderBillingStatusAsync(invoice);
            await _repo.SaveChangesAsync();
            await _notifications.NotifyInvoiceEventAsync(
                invoice.LoadId,
                userId,
                $"Invoice {invoice.InvoiceNumber} status changed to {status}");
        }

        public async Task<CustomerInvoiceDto> RecordPaymentAsync(Guid invoiceId, RecordInvoicePaymentDto dto, Guid userId)
        {
            if (!await _permission.HasPermissionAsync(userId, Permission.Financial_Invoice_RecordPayment))
                throw new ForbiddenException("You are not allowed to record invoice payments.");

            var invoice = await _repo.GetByIdAsync(invoiceId)
                ?? throw new NotFoundException("Invoice not found.");

            var amountPaid = dto.AmountPaid < 0 ? 0 : dto.AmountPaid;
            invoice.AmountPaid = amountPaid > invoice.TotalAmount ? invoice.TotalAmount : amountPaid;
            invoice.PaidAt = invoice.AmountPaid >= invoice.TotalAmount
                ? dto.PaidAt ?? DateTime.UtcNow
                : dto.PaidAt;
            invoice.PaymentReference = dto.PaymentReference;
            invoice.Status = invoice.AmountPaid >= invoice.TotalAmount
                ? InvoiceStatus.Paid
                : InvoiceStatus.Sent;
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedByUserId = userId;

            await SyncOrderBillingStatusAsync(invoice);
            await _repo.SaveChangesAsync();
            await _notifications.NotifyInvoiceEventAsync(
                invoice.LoadId,
                userId,
                $"Payment recorded for invoice {invoice.InvoiceNumber}: {invoice.AmountPaid:N2}");
            return Map(invoice);
        }

        public async Task UpdatePdfUrlAsync(Guid invoiceId, string pdfUrl)
        {
            var invoice = await _repo.GetByIdAsync(invoiceId)
                ?? throw new NotFoundException("Invoice not found.");

            invoice.PdfUrl = pdfUrl;

            await _repo.SaveChangesAsync();
        }

        private static CustomerInvoiceDto Map(CustomerInvoice invoice)
        {
            return new CustomerInvoiceDto
            {
                Id = invoice.Id,
                LoadId = invoice.LoadId,
                CustomerId = invoice.CustomerId,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                InvoiceType = invoice.InvoiceType,
                Status = invoice.Status,
                TotalAmount = invoice.TotalAmount,
                AmountPaid = invoice.AmountPaid,
                BalanceDue = invoice.TotalAmount - invoice.AmountPaid,
                PaidAt = invoice.PaidAt,
                PaymentReference = invoice.PaymentReference,
                Notes = invoice.Notes,
                PdfUrl = invoice.PdfUrl,

                LineItems = invoice.LineItems
                    .Select(x => new InvoiceLineItemDto
                    {
                        Description = x.Description,
                        Qty = x.Qty,
                        Price = x.Price,
                        Amount = x.Amount
                    })
                    .ToList()
            };
        }
        private async Task RecalculateDraftInvoiceAsync(CustomerInvoice invoice, Load load)
        {
            // Lejo refresh vetëm në Draft
            if (invoice.Status != InvoiceStatus.Draft)
                return;

            var items = BuildLineItemsFromLoad(load);

            await _repo.DeleteLineItemsByInvoiceIdAsync(invoice.Id);
            foreach (var item in items)
            {
                item.InvoiceId = invoice.Id;
            }
            if (items.Count > 0)
            {
                await _repo.AddLineItemsAsync(items);
            }

            invoice.TotalAmount = items.Sum(x => x.Amount);
            if (invoice.AmountPaid > invoice.TotalAmount)
                invoice.AmountPaid = invoice.TotalAmount;

            invoice.DueDate = invoice.InvoiceDate.AddDays((int)load.Customer.Billing.Terms);
        }

        private async Task SyncOrderBillingStatusAsync(CustomerInvoice invoice)
        {
            if (invoice.Load?.Orders == null || invoice.Load.Orders.Count == 0)
                return;

            var targetStatus = invoice.Status switch
            {
                InvoiceStatus.Sent => OrderStatus.Billed,
                InvoiceStatus.Paid => OrderStatus.Completed,
                _ => (OrderStatus?)null
            };

            if (!targetStatus.HasValue)
                return;

            foreach (var orderId in invoice.Load.Orders.Select(x => x.OrderId).Distinct())
            {
                var order = await _orders.GetByIdWithLoadsAsync(orderId);
                order?.TrySyncStatusFromExecution(targetStatus.Value);
            }
        }

    }
}
