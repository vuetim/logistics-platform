using LogisticsPlatform.Domain.Entities.Financial;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LogisticsPlatform.Infrastructure.Services.Documents
{
    public class CarrierSettlementPdfDocument : IDocument
    {
        private readonly CarrierSettlement _settlement;

        public CarrierSettlementPdfDocument(CarrierSettlement settlement)
        {
            _settlement = settlement;
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

        // HEADER ==========================================================
        private void ComposeHeader(IContainer container)
        {
            var carrierName = _settlement.Carrier?.Name ?? "Carrier";
            var loadNumber = _settlement.Load?.LoadNumber ?? "-";

            container.Row(row =>
            {
                // LEFT SIDE - COMPANY INFO
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("LogisticsPlatform").FontSize(16).SemiBold();
                    col.Item().Text("Your Company Address");
                    col.Item().Text("City, Country");
                });

                // RIGHT SIDE – DOCUMENT INFO
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().AlignRight().Text("CARRIER SETTLEMENT").FontSize(20).SemiBold();
                    col.Item().AlignRight().Text($"#{_settlement.SettlementNumber}");
                    col.Item().AlignRight().Text($"Date: {_settlement.SettlementDate:yyyy-MM-dd}");
                    col.Item().AlignRight().Text($"Terms: Net {_settlement.Carrier.PaymentTermsDays}");

                    col.Item().AlignRight().Text($"Status: {_settlement.Status}");
                    col.Item().AlignRight().Text($"Load: {loadNumber}");
                    col.Item().AlignRight().Text($"Pay To: {carrierName}");
                });
            });
        }

        // CONTENT ==========================================================
        private void ComposeContent(IContainer container)
        {
            var carrierName = _settlement.Carrier?.Name ?? "Carrier";
            var carrierId = _settlement.CarrierId.ToString();

            var origin = _settlement.Load?.Origin ?? "-";
            var destination = _settlement.Load?.Destination ?? "-";

            var pickup = _settlement.Load?.Stops
                ?.Where(s => s.StopType == Domain.Enums.StopType.Pickup)
                .OrderBy(s => s.PlannedArrivalFrom)
                .FirstOrDefault();

            var delivery = _settlement.Load?.Stops
                ?.Where(s => s.StopType == Domain.Enums.StopType.Delivery)
                .OrderBy(s => s.PlannedArrivalFrom)
                .FirstOrDefault();

            container.Column(col =>
            {
                col.Spacing(15);

                // PAY TO + SHIPMENT SUMMARY
                col.Item().Row(row =>
                {
                    // PAY TO
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Pay To").SemiBold();
                        c.Item().Text(carrierName);
                        c.Item().Text($"Carrier ID: {carrierId}");
                    });

                    // SHIPMENT SUMMARY
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

                // CHARGES TABLE
                col.Item().Element(ComposeChargesTable);

                // TOTAL
                col.Item().AlignRight().Text(text =>
                {
                    text.Span("Total: ").SemiBold();
                    text.Span(_settlement.TotalAmount.ToString("C2"));
                });

                // LOAD ITEMS (optional)
                if (_settlement.Load?.Items != null && _settlement.Load.Items.Any())
                {
                    col.Item().PaddingTop(20).Text("Shipment Items").FontSize(12).SemiBold();
                    col.Item().Element(ComposeShipmentItemsTable);
                }
            });
        }

        // TABLE: Settlement Line Items =====================================
        private void ComposeChargesTable(IContainer container)
        {
            var lines = _settlement.LineItems;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(5); // Description
                    columns.RelativeColumn(1); // Qty
                    columns.RelativeColumn(2); // Price
                    columns.RelativeColumn(2); // Amount
                });

                // Header Row
                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("Description");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Qty");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Price");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Amount");

                    static IContainer HeaderStyle(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold())
                                 .PaddingBottom(5)
                                 .BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2);
                });

                // Body Rows
                foreach (var line in lines)
                {
                    table.Cell().Element(BodyStyle).Text(line.Description);
                    table.Cell().Element(BodyStyle).AlignRight().Text(line.Qty.ToString("0.##"));
                    table.Cell().Element(BodyStyle).AlignRight().Text(line.Price.ToString("0.00"));
                    table.Cell().Element(BodyStyle).AlignRight().Text(line.Amount.ToString("0.00"));
                }

                static IContainer BodyStyle(IContainer container) =>
                    container.PaddingVertical(2);
            });
        }

        // SHIPMENT ITEMS TABLE =============================================
        private void ComposeShipmentItemsTable(IContainer container)
        {
            var items = _settlement.Load!.Items;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(4);
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("Item");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Qty");
                    header.Cell().Element(HeaderStyle).Text("Unit");
                    header.Cell().Element(HeaderStyle).AlignCenter().Text("Hazmat");
                    header.Cell().Element(HeaderStyle).Text("Notes");

                    static IContainer HeaderStyle(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold())
                                 .PaddingBottom(5)
                                 .BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2);
                });

                // Rows
                foreach (var item in items)
                {
                    table.Cell().Element(BodyStyle).Text(item.Name);
                    table.Cell().Element(BodyStyle).AlignRight().Text(item.Quantity.ToString("0.##"));
                    table.Cell().Element(BodyStyle).Text(item.QuantityUnit ?? "");
                    table.Cell().Element(BodyStyle).AlignCenter().Text(item.IsHazmat ? "YES" : "NO");
                    table.Cell().Element(BodyStyle).Text(item.Notes ?? "");
                }

                static IContainer BodyStyle(IContainer container) =>
                    container.PaddingVertical(2);
            });
        }

        // FOOTER ==============================================================
        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text("Thank you for your partnership.")
                .FontSize(9)
                .FontColor(Colors.Grey.Darken1);
        }
    }
}
