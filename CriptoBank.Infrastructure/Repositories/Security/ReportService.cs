
using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.ReportService;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CriptoBank.Infrastructure.Repositories.Security
{
    public class ReportService : IReportService
    {
        public byte[] GenerateTransactionReport(string userName, List<TransactionReportDTO> transactions)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, Unit.Centimetre);
                    page.Header().Text($"Extrato CriptoBank - {userName}").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns => {
                            columns.RelativeColumn(); columns.RelativeColumn();
                            columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn();
                        });

                        table.Header(header => {
                            header.Cell().Element(CellStyle).Text("Data");
                            header.Cell().Element(CellStyle).Text("Tipo");
                            header.Cell().Element(CellStyle).Text("Moeda");
                            header.Cell().Element(CellStyle).Text("Qtd");
                            header.Cell().Element(CellStyle).Text("Total");

                            static IContainer CellStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1);
                        });

                        foreach (var tx in transactions)
                        {
                            table.Cell().Text(tx.Date.ToShortDateString());
                            table.Cell().Text(tx.Type);
                            table.Cell().Text(tx.CryptoName);
                            table.Cell().Text(tx.Quantity.ToString("F8"));
                            table.Cell().Text(tx.TotalValue.ToString("C"));
                        }
                    });
                });
            }).GeneratePdf();
        }
    }
}
