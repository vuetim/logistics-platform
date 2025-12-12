using LogisticsPlatform.Domain.Entities.Financial;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LogisticsPlatform.Infrastructure.Services.Documents
{
    public class InvoicePdfDocument : IDocument
    {
        private readonly CustomerInvoice _invoice;

        public InvoicePdfDocument(CustomerInvoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        // HEADER: Company + Invoice Info
        private void ComposeHeader(IContainer container)
        {
            var customerName = _invoice.Customer?.Name ?? "Customer";
            var loadNumber = _invoice.Load?.LoadNumber ?? "-";

            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("LogisticsPlatform").FontSize(16).SemiBold();
                    col.Item().Text("Your Company Address");
                    col.Item().Text("City, Country");
                });

                row.ConstantItem(200).Column(col =>
                {
                    col.Item().AlignRight().Text("INVOICE").FontSize(20).SemiBold();
                    col.Item().AlignRight().Text($"#{_invoice.InvoiceNumber}");
                    col.Item().AlignRight().Text($"Date: {_invoice.InvoiceDate:yyyy-MM-dd}");
                    if (_invoice.DueDate != null)
                        col.Item().AlignRight().Text($"Terms: Net {_invoice.Customer.PaymentTermsDays}");
                    if (_invoice.DueDate != null)
                        col.Item().AlignRight().Text($"Due: {_invoice.DueDate:yyyy-MM-dd}");
                    col.Item().AlignRight().Text($"Status: {_invoice.Status}");
                    col.Item().AlignRight().Text($"Load: {loadNumber}");
                    col.Item().AlignRight().Text($"Bill To: {customerName}");
                });
            });
        }

        // CONTENT: Bill To + Shipment Info + Charges + Shipment Items
        private void ComposeContent(IContainer container)
        {
            var customerName = _invoice.Customer?.Name ?? "Customer";

            var origin = _invoice.Load?.Origin ?? "-";
            var destination = _invoice.Load?.Destination ?? "-";

            var pickup = _invoice.Load?.Stops
                ?.Where(s => s.StopType == Domain.Enums.StopType.Pickup)
                .OrderBy(s => s.PlannedArrivalFrom)
                .FirstOrDefault();

            var delivery = _invoice.Load?.Stops
                ?.Where(s => s.StopType == Domain.Enums.StopType.Delivery)
                .OrderBy(s => s.PlannedArrivalFrom)
                .FirstOrDefault();

            container.Column(col =>
            {
                col.Spacing(15);

                // BILL TO + SHIPMENT SUMMARY
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Bill To").SemiBold();
                        c.Item().Text(customerName);
                    });

                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Shipment").SemiBold();
                        c.Item().Text($"Origin: {origin}");
                        c.Item().Text($"Destination: {destination}");
                        if (pickup != null)
                            c.Item().Text($"Pickup: {pickup.City}, {pickup.State} ({pickup.PlannedArrivalFrom:yyyy-MM-dd})");
                        if (delivery != null)
                            c.Item().Text($"Delivery: {delivery.City}, {delivery.State} ({delivery.PlannedArrivalFrom:yyyy-MM-dd})");
                    });
                });

                // CHARGES TABLE (BILLABLE LINES)
                col.Item().Element(ComposeChargesTable);

                // TOTAL
                col.Item().AlignRight().Text(text =>
                {
                    text.Span("Total: ").SemiBold();
                    text.Span(_invoice.TotalAmount.ToString("C2"));
                });








                // SHIPMENT ITEMS TABLE (banane, paleta, etj.) – informativ
                if (_invoice.Load?.Items != null && _invoice.Load.Items.Any())
                {
                    col.Item().PaddingTop(20).Text("Shipment Items").FontSize(12).SemiBold();
                    col.Item().Element(ComposeShipmentItemsTable);
                }
            });
        }

        // CHARGES: Description, Qty, Price, Amount
        private void ComposeChargesTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(5); // Description
                    columns.RelativeColumn(1); // Qty
                    columns.RelativeColumn(2); // Price
                    columns.RelativeColumn(2); // Amount
                });

                // Header row
                table.Header(header =>
                {
                    header.Cell().Element(CellHeader).Text("Description");
                    header.Cell().Element(CellHeader).AlignRight().Text("Qty");
                    header.Cell().Element(CellHeader).AlignRight().Text("Price");
                    header.Cell().Element(CellHeader).AlignRight().Text("Amount");

                    static IContainer CellHeader(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold())
                                 .PaddingBottom(5)
                                 .BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2);
                });

                foreach (var line in _invoice.LineItems)
                {
                    table.Cell().Element(CellBody).Text(line.Description);
                    table.Cell().Element(CellBody).AlignRight().Text(line.Qty.ToString("0.##"));
                    table.Cell().Element(CellBody).AlignRight().Text(line.Price.ToString("0.00"));
                    table.Cell().Element(CellBody).AlignRight().Text(line.Amount.ToString("0.00"));
                }

                static IContainer CellBody(IContainer container) =>
                    container.PaddingVertical(2);
            });
        }

        // SHIPMENT ITEMS: Name, Qty, Unit, Hazmat, Notes
        private void ComposeShipmentItemsTable(IContainer container)
        {
            var items = _invoice.Load!.Items;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4); // Name
                    columns.RelativeColumn(1); // Qty
                    columns.RelativeColumn(2); // Unit
                    columns.RelativeColumn(1); // Hazmat
                    columns.RelativeColumn(4); // Notes
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(CellHeader).Text("Item");
                    header.Cell().Element(CellHeader).AlignRight().Text("Qty");
                    header.Cell().Element(CellHeader).Text("Unit");
                    header.Cell().Element(CellHeader).AlignCenter().Text("Hazmat");
                    header.Cell().Element(CellHeader).Text("Notes");

                    static IContainer CellHeader(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold())
                                 .PaddingBottom(5)
                                 .BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2);
                });

                foreach (var item in items)
                {
                    table.Cell().Element(CellBody).Text(item.Name);
                    table.Cell().Element(CellBody).AlignRight().Text(item.Quantity.ToString("0.##"));
                    table.Cell().Element(CellBody).Text(item.QuantityUnit ?? "");
                    table.Cell().Element(CellBody).AlignCenter().Text(item.IsHazmat ? "YES" : "NO");
                    table.Cell().Element(CellBody).Text(item.Notes ?? "");
                }

                static IContainer CellBody(IContainer container) =>
                    container.PaddingVertical(2);
            });
        }

        // FOOTER
        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text("Thank you for your business.")
                .FontSize(9)
                .FontColor(Colors.Grey.Darken1);
        }
    }
}
