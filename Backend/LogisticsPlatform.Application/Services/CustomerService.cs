using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.DTOs.Customers.Addresses;
using LogisticsPlatform.Application.DTOs.Customers.Contacts;
using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customers;
        private readonly IUnitOfWork _uow;

        public CustomerService(
            ICustomerRepository customers,
            IUnitOfWork uow)
        {
            _customers = customers;
            _uow = uow;
        }

        // ======================
        // GET ALL
        // ======================
        public async Task<IEnumerable<CustomerListItemDto>> GetAllAsync()
        {
            var list = await _customers.GetAllAsync();

            return list.Select(c => new CustomerListItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                IsActive = c.IsActive
            });
        }

        // ======================
        // GET BY ID
        // ======================
        public async Task<CustomerListItemDto?> GetByIdAsync(Guid id)
        {
            var c = await _customers.GetByIdAsync(id);
            if (c == null) return null;

            return new CustomerListItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                IsActive = c.IsActive
            };
        }

        // ======================
        // GET DETAILS (aggregate)
        // ======================
        public async Task<CustomerDetailsDto?> GetDetailsAsync(Guid id)
        {
            var c = await _customers.GetDetailsAsync(id);
            if (c == null) return null;

            return new CustomerDetailsDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                PaymentTermsDays = c.PaymentTermsDays,
                IsActive = c.IsActive,

                Addresses = c.Addresses
                    .Where(a => a.IsActive)
                    .Select(a => new CustomerAddressDto
                    {
                        Id = a.Id,
                        AddressLine1 = a.AddressLine1,
                        City = a.City,
                        Country = a.Country,
                        IsPrimary = a.IsPrimary,
                        Type = a.Type
                    }).ToList(),

                Contacts = c.Contacts
                    .Where(x => x.IsActive)
                    .Select(x => new CustomerContactDto
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        Phone = x.Phone,
                        Position = x.Position,
                        IsPrimary = x.IsPrimary
                    }).ToList(),

                Notes = c.Notes
                    .OrderByDescending(n => n.CreatedAt)
                    .Select(n => new CustomerNoteDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        CreatedAt = n.CreatedAt
                    }).ToList()
            };
        }

        // ======================
        // CREATE BASIC
        // ======================
        public async Task<Guid> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer(
                dto.Name,
                dto.Email,
                dto.Phone,
                dto.PaymentTermsDays,
                true
            );

            await _customers.AddAsync(customer);
            await _uow.SaveChangesAsync();

            return customer.Id;
        }

        // ======================
        // UPDATE
        // ======================
        public async Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto)
        {
            var customer = await _customers.GetByIdAsync(id);
            if (customer == null) return false;

            customer.UpdateBasicInfo(
                dto.Name,
                dto.Email,
                dto.Phone,
                dto.PaymentTermsDays
            );

            _customers.Update(customer);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ======================
        // DELETE (soft)
        // ======================
        public async Task<bool> DeleteAsync(Guid id)
        {
            var customer = await _customers.GetByIdAsync(id);
            if (customer == null) return false;

            customer.Deactivate();

            _customers.Update(customer);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ======================
        // CREATE FULL (DDD aggregate)
        // ======================
        public async Task<Guid> CreateFullAsync(CreateCustomerFullDto dto, Guid userId)
        {
            await _uow.BeginAsync();

            try
            {
                var customer = new Customer(
                    dto.Customer.Name,
                    dto.Customer.Email,
                    dto.Customer.Phone,
                    dto.Customer.PaymentTermsDays,
                    dto.Customer.IsActive
                );

                // Addresses
                foreach (var a in dto.Addresses)
                {
                    customer.AddAddress(new CustomerAddress
                    {
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        City = a.City,
                        State = a.State,
                        Country = a.Country,
                        PostalCode = a.PostalCode,
                        Type = a.Type,
                        IsPrimary = a.IsPrimary,
                        IsActive = true
                    });
                }

                // Contacts
                foreach (var c in dto.Contacts)
                {
                    customer.AddContact(new CustomerContact
                    {
                        FullName = c.FullName,
                        Email = c.Email,
                        Phone = c.Phone,
                        Position = c.Position,
                        IsPrimary = c.IsPrimary,
                        IsActive = true
                    });
                }

                // Notes
                foreach (var n in dto.Notes)
                {
                    customer.AddNote(new CustomerNote
                    {
                        Title = n.Title,
                        Message = n.Message,
                        CreatedByUserId = userId
                    });
                }

                await _customers.AddAsync(customer);

                await _uow.CommitAsync();

                return customer.Id;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
