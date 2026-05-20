using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LogisticsPlatform.Infrastructure.Services.Documents;

public class RateConfirmationService : IRateConfirmationService
{
    public byte[] GeneratePdf(LoadCarrierAssignment assignment)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(36);
                page.Size(PageSizes.Letter);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("RATE CONFIRMATION").FontSize(18).Bold();
                    col.Item().Text($"Load {assignment.Load.LoadNumber}");
                });

                page.Content().PaddingTop(16).Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text($"Carrier: {assignment.Carrier.Name}").Bold();
                    col.Item().Text($"Rate: {(assignment.OfferedRate ?? 0):N2} {assignment.Currency ?? "USD"}");
                    col.Item().Text($"Confirmation #: {assignment.RateConfirmationNumber ?? assignment.Load.LoadNumber}");
                    col.Item().Text($"Lane: {assignment.Load.Origin} to {assignment.Load.Destination}");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("#").Bold();
                            header.Cell().Text("Type").Bold();
                            header.Cell().Text("Location").Bold();
                            header.Cell().Text("Window").Bold();
                        });

                        foreach (var stop in assignment.Load.Stops.OrderBy(x => x.Sequence))
                        {
                            table.Cell().Text(stop.Sequence.ToString());
                            table.Cell().Text(stop.StopType.ToString());
                            table.Cell().Text($"{stop.LocationName} {stop.City}, {stop.State}");
                            table.Cell().Text($"{stop.PlannedArrivalFrom:g} - {stop.PlannedArrivalTo:g}");
                        }
                    });

                    if (!string.IsNullOrWhiteSpace(assignment.TenderNotes))
                    {
                        col.Item().Text("Notes").Bold();
                        col.Item().Text(assignment.TenderNotes);
                    }

                    col.Item().PaddingTop(18).Text("Carrier agrees to perform transportation under the offered rate and tender terms. Driver, equipment, and check-in details must be provided before dispatch when required.").FontSize(9);
                });
            });
        }).GeneratePdf();
    }
}
