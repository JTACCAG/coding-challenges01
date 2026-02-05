using Api.Domain.Entities;
using Api.Domain.ValueObjects;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Application.Services
{
    public class InventoryReport : IDocument
    {
        private readonly List<ReportWithProductDto> _reports;

        public InventoryReport(List<ReportWithProductDto> reports)
        {
            _reports = reports;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(Header);
                page.Content().Element(Content);
                page.Footer().AlignCenter().Text(x =>
                {
                    //x.Span("Reporte generado el ");
                    //x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }

        void Header(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("REPORTE DE INVENTARIO BAJO")
                        .FontSize(18)
                        .Bold();

                    col.Item().Text("Productos con stock por debajo del mínimo")
                        .FontColor(Colors.Grey.Darken1);
                });
            });
        }

        void Content(IContainer container)
        {
            container.PaddingTop(20).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(4);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellHeader).Text("Nombre").Bold();
                    header.Cell().Element(CellHeader).Text("Descripción").Bold();
                    header.Cell().Element(CellHeader).Text("Precio").Bold();
                    header.Cell().Element(CellHeader).Text("Stock").Bold();
                    header.Cell().Element(CellHeader).Text("Fecha creacion").Bold();

                    static IContainer CellHeader(IContainer container) =>
                        container.Padding(5).Background(Colors.Grey.Lighten3).BorderBottom(1);
                });

                foreach (var p in _reports)
                {
                    table.Cell().Element(Cell).Text(p.Product.Name);
                    table.Cell().Element(Cell).Text(p.Product.Description);
                    table.Cell().Element(Cell).AlignRight().Text(p.Product.Price.ToString());
                    table.Cell().Element(Cell).AlignRight().Text(p.Product.StockQuantity.ToString());
                    var fecha = p.Product.CreatedAt.ToString().Substring(0,10);
                    var day = fecha.Split('/')[0];
                    var month = fecha.Split('/')[1];
                    var year = fecha.Split('/')[2];

                    table.Cell().Element(Cell).AlignRight().Text(month + '-' + day + '-' + year);
                }

                static IContainer Cell(IContainer container) =>
                    container.Padding(5).BorderBottom(1);
            });
        }
    }
}
