using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SmartBooks.Domain.Entities;
using System.IO;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Infrastructure.PDF
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GenerarFacturaPdf(Venta venta)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var logoPath = Path.Combine(AppContext.BaseDirectory, "PDF", "logocecar.png");

            var primaryColor = Color.FromHex("#e12d2e"); 
            var secondaryColor = Color.FromHex("#4142a1"); 

            var lightGray = Color.FromHex("#f8f9fa");
            var borderColor = Color.FromHex("#e0e0e0");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.Letter);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Helvetica"));

                    page.Header().Column(headerColumn =>
                    {
                        headerColumn.Spacing(20);

                        headerColumn.Item().Row(row =>
                        {
                            if (File.Exists(logoPath))
                            {
                                row.ConstantItem(120).Image(logoPath).FitArea();
                            }

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text("FACTURA DE VENTA")
                                    .FontSize(20)
                                    .SemiBold()
                                    .FontColor(primaryColor);

                                col.Item().Text($"N° {venta.NumeroRecibo}")
                                    .FontSize(12)
                                    .FontColor(secondaryColor);

                                col.Item().Text($"Comprobante: {venta.NumeroComprobante}")
                                    .FontSize(12)
                                    .FontColor(secondaryColor) 
                                    .SemiBold();
                            });
                        });

                        headerColumn.Item().LineHorizontal(1).LineColor(borderColor);
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingVertical(25).Column(mainContent =>
                        {
                            mainContent.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Column(column =>
                                {
                                    column.Item().Text("INFORMACIÓN DE FACTURA")
                                        .FontSize(12)
                                        .SemiBold()
                                        .FontColor(primaryColor); 

                                    column.Item().PaddingTop(5).Text($"Fecha: {venta.Fecha:dd/MM/yyyy}");
                                    column.Item().Text($"Hora: {venta.Fecha:HH:mm}");
                                    column.Item().Text("Estado: Pagado")
                                          .SemiBold()
                                          .FontColor(primaryColor); 

                                    column.Item().Text($"Comprobante N°: {venta.NumeroComprobante}")
                                          .FontColor(secondaryColor) 
                                          .SemiBold();
                                });

                                table.Cell().Column(column =>
                                {
                                    column.Item().Text("INFORMACIÓN DEL CLIENTE")
                                        .FontSize(12)
                                        .SemiBold()
                                        .FontColor(primaryColor);

                                    column.Item().PaddingTop(5).Text(venta.Cliente.Nombres);
                                    column.Item().Text($"Documento: {venta.Cliente.Identificacion}");
                                    column.Item().Text($"Contacto: {venta.Cliente.Celular}");
                                });
                            });

                            mainContent.Item().PaddingTop(30);

                            mainContent.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(100);
                                    columns.ConstantColumn(100);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(secondaryColor).Padding(10)
                                          .Text("PRODUCTO")
                                          .SemiBold()
                                          .FontColor(Colors.White);

                                    header.Cell().Background(secondaryColor).Padding(10)
                                          .Text("CANTIDAD")
                                          .SemiBold()
                                          .FontColor(Colors.White);

                                    header.Cell().Background(secondaryColor).Padding(10)
                                          .Text("PRECIO UNIT.")
                                          .SemiBold()
                                          .FontColor(Colors.White);

                                    header.Cell().Background(secondaryColor).Padding(10)
                                          .Text("SUBTOTAL")
                                          .SemiBold()
                                          .FontColor(Colors.White);
                                });

                                foreach (var item in venta.Items)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(borderColor)
                                         .PaddingVertical(10).PaddingLeft(10)
                                         .Text(item.Libro.Nombre);

                                    table.Cell().BorderBottom(1).BorderColor(borderColor)
                                         .PaddingVertical(10)
                                         .AlignCenter()
                                         .Text(item.Cantidad.ToString());

                                    table.Cell().BorderBottom(1).BorderColor(borderColor)
                                         .PaddingVertical(10).PaddingLeft(10)
                                         .Text(item.PrecioUnitario.ToString("C"));

                                    table.Cell().BorderBottom(1).BorderColor(borderColor)
                                         .PaddingVertical(10).PaddingLeft(10)
                                         .Text(item.Subtotal.ToString("C"));
                                }
                            });

                            mainContent.Item().PaddingTop(20);

                            mainContent.Item().AlignRight().Width(250).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(120);
                                });

                                table.Cell().PaddingVertical(10).PaddingLeft(10).Text("Subtotal:").SemiBold();
                                table.Cell().PaddingVertical(10).PaddingRight(10).AlignRight()
                                     .Text(venta.Items.Sum(x => x.Subtotal).ToString("C"));

                                table.Cell().PaddingVertical(10).PaddingLeft(10).Text("IVA (0%):").SemiBold();
                                table.Cell().PaddingVertical(10).PaddingRight(10).AlignRight().Text("$0.00");

                                table.Cell().ColumnSpan(2).PaddingVertical(10).LineHorizontal(1).LineColor(borderColor);

                                table.Cell().PaddingVertical(10).PaddingLeft(10)
                                    .Text("TOTAL:")
                                    .FontSize(13)
                                    .SemiBold()
                                    .FontColor(primaryColor);

                                table.Cell().PaddingVertical(10).PaddingRight(10)
                                    .AlignRight()
                                    .Text(venta.Total.ToString("C"))
                                    .FontSize(13)
                                    .SemiBold()
                                    .FontColor(primaryColor);
                            });
                        });

                        col.Item().PaddingTop(170).AlignCenter().Text(text =>
                        {
                            text.Span("Esta factura es un comprobante válido de compra. ")
                                .FontColor(Colors.Grey.Medium).FontSize(9);
                            text.Span("Conserve este documento para cualquier consulta. ")
                                .FontColor(Colors.Grey.Medium).FontSize(9);
                            text.Span("Para soporte contacte a servicio al cliente.")
                                .FontColor(Colors.Grey.Medium).FontSize(9);
                        });
                    });

                    page.Footer().Column(footerCol =>
                    {
                        footerCol.Item().LineHorizontal(1).LineColor(borderColor);

                        footerCol.Item().PaddingTop(15).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("SmartBook Education System")
                                    .SemiBold()
                                    .FontColor(primaryColor);

                                col.Item().Text("contacto@smartbook.edu.co | www.smartbook.edu.co")
                                   .FontSize(9)
                                   .FontColor(Colors.Grey.Medium);
                            });

                            row.ConstantItem(200).AlignRight().Column(col =>
                            {
                                col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                   .FontSize(9)
                                   .FontColor(secondaryColor);
                            });
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
