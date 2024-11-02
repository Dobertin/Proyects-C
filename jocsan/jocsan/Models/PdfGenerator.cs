using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
namespace jocsan.Models
{
    public class PdfGenerator
    {
        public byte[] GenerarFacturaPdf(Factura factura)
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Configuración de la fuente
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var textFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Título
                var title = new Paragraph("DIOS TE AMA NO LO DUDES | # Factura: " + factura.IdFactura, titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Espacio
                document.Add(new Paragraph(" "));

                // Información del capitán y datos principales
                var infoTable = new PdfPTable(2);
                infoTable.AddCell(new PdfPCell(new Phrase("Capitán:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.Cliente.Capitan, textFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase("Dueño:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.Cliente.Nombre, textFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase("Porcentaje:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.Porcentaje + "%", textFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase("Fecha:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.FechaVenta.ToShortDateString(), textFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase("Galón:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.Cliente.Gasolina.ToString(), textFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase("Galones:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.Galones.ToString(), textFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase("Hielo:", subtitleFont)) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase(factura.Hielo.ToString(), textFont)) { Border = 0 });
                document.Add(infoTable);

                // Espacio
                document.Add(new Paragraph(" "));

                // Tabla de productos
                var productosTable = new PdfPTable(4);
                productosTable.AddCell(new PdfPCell(new Phrase("Producto", subtitleFont)));
                productosTable.AddCell(new PdfPCell(new Phrase("Cantidad", subtitleFont)));
                productosTable.AddCell(new PdfPCell(new Phrase("Precio", subtitleFont)));
                productosTable.AddCell(new PdfPCell(new Phrase("Total", subtitleFont)));

                foreach (var detalle in factura.DetalleFacturas)
                {
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.Producto.NombreLocal, textFont)));
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.Cantidad.ToString(), textFont)));
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.PrecioUnitario.ToString("C"), textFont)));
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.TotalParcial.ToString("C"), textFont)));
                }

                document.Add(productosTable);

                // Espacio
                document.Add(new Paragraph(" "));

                // Subtotales y totales
                var totalsTable = new PdfPTable(2);
                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal Producto:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.SubTotalProd.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("G+H:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.GH.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal GH:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.SubTotalGH.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Tercero:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Terceros.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Peladores:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Peladores.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("25% =", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Valor25.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("13% =", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Valor13.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal =", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.SubTotal.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Cantidad de Abono:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Abono.ToString("C"), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Total:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.TotalVenta.ToString("C"), textFont)) { Border = 0 });
                document.Add(totalsTable);
                // Espacio
                document.Add(new Paragraph(" "));
                // Versículo
                var verse = new Paragraph("1 Juan 3:16 - Porque de tal manera amó Dios al mundo, que ha dado a su Hijo unigénito, para que todo aquel que en él cree, no se pierda, mas tenga vida eterna.", textFont);
                verse.Alignment = Element.ALIGN_CENTER;
                document.Add(verse);

                document.Close();
                return ms.ToArray();
            }
        }
        public byte[] GenerarCreditoPdf(Credito credito)
        {
            using (var ms = new MemoryStream())
            {
                // Crear el documento PDF con tamaño de página predeterminado
                var document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Fuente para el contenido
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var fontContenido = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var fontVersiculo = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10);

                // Título
                var titulo = new Paragraph("Comprobante de crédito", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titulo);

                // Contenido del comprobante
                var contenido = new Paragraph
                {
                    Alignment = Element.ALIGN_CENTER
                };
                contenido.Add(new Phrase($"Descripción: {credito.Descripcion}\n", fontContenido));
                contenido.Add(new Phrase($"Fecha: {credito.FechaCredito:dd/MM/yyyy}\n", fontContenido));
                contenido.Add(new Phrase($"Monto: {credito.ValorCredito.ToString("C")}\n", fontContenido));
                contenido.Add(new Phrase($"Cliente: {credito.Cliente.Nombre}\n", fontContenido));
                document.Add(contenido);

                // Espacio adicional antes del versículo
                document.Add(new Paragraph("\n\n"));

                // Versículo
                var versiculo = new Paragraph("Romanos 13:7a y 8a", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 5
                };
                document.Add(versiculo);

                var textoVersiculo = new Paragraph("No tengan deudas pendientes con nadie a no ser la de amarse unos a otros", fontVersiculo)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(textoVersiculo);

                // Cerrar el documento
                document.Close();

                // Retornar el PDF como array de bytes
                return ms.ToArray();
            }
        }
    }
}