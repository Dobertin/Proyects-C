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
                // Definir tamaño de página personalizado de 80 mm de ancho
                var pageWidth = 80f * 2.83f; // 80 mm en puntos
                var pageSize = new Rectangle(pageWidth, PageSize.A4.Height); // Mantener la altura ajustable para contenido largo
                var document = new Document(pageSize, 10, 10, 10, 10);

                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Configuración de las fuentes en tamaños más pequeños
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
                var textFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);

                // Título
                var title = new Paragraph("DIOS TE AMA NO LO DUDES | # Factura: " + factura.IdFactura, titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph(" "));

                // Información del capitán y datos principales
                var infoTable = new PdfPTable(2) { WidthPercentage = 100 };
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

                document.Add(new Paragraph(" "));

                // Tabla de productos
                var productosTable = new PdfPTable(4) { WidthPercentage = 100 };
                productosTable.AddCell(new PdfPCell(new Phrase("Producto", subtitleFont)));
                productosTable.AddCell(new PdfPCell(new Phrase("Cant.", subtitleFont)));
                productosTable.AddCell(new PdfPCell(new Phrase("Precio", subtitleFont)));
                productosTable.AddCell(new PdfPCell(new Phrase("Total", subtitleFont)));

                foreach (var detalle in factura.DetalleFacturas)
                {
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.Producto.NombreLocal, textFont)));
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.Cantidad.ToString(), textFont)));
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.PrecioUnitario.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)));
                    productosTable.AddCell(new PdfPCell(new Phrase(detalle.TotalParcial.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)));
                }
                document.Add(productosTable);

                document.Add(new Paragraph(" "));

                // Tabla de totales
                var totalsTable = new PdfPTable(2) { WidthPercentage = 100 };
                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal Producto:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.SubTotalProd.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("G+H:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.GH.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal GH:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.SubTotalGH.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Tercero:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Terceros.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Peladores:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Peladores.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("25% =", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Valor25.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("13% =", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Valor13.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal =", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.SubTotal.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Cantidad de Abono:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.Abono.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase("Total:", subtitleFont)) { Border = 0 });
                totalsTable.AddCell(new PdfPCell(new Phrase(factura.TotalVenta.ToString("C", new System.Globalization.CultureInfo("es-CR")), textFont)) { Border = 0 });
                document.Add(totalsTable);

                document.Add(new Paragraph(" "));

                // Versículo
                var verse = new Paragraph("1 Juan 3:16 - Porque de tal manera amó Dios al mundo...", textFont);
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
                // Definir un tamaño de página personalizado de 80 mm de ancho
                var pageWidth = 80f * 2.83f; // 80 mm en puntos (aprox. 3.15 pulgadas)
                var pageSize = new Rectangle(pageWidth, PageSize.A7.Height); // Ajuste la altura según sea necesario
                var document = new Document(pageSize, 10, 10, 10, 10); // Márgenes de 10 puntos

                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Fuente para el contenido con tamaños reducidos
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontContenido = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                var fontVersiculo = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 7);

                // Título
                var titulo = new Paragraph("Comprobante de crédito", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15
                };
                document.Add(titulo);

                // Contenido del comprobante
                var contenido = new Paragraph
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 10
                };
                contenido.Add(new Phrase($"Descripción: {credito.Descripcion}\n", fontContenido));
                contenido.Add(new Phrase($"Fecha: {credito.FechaCredito:dd/MM/yyyy}\n", fontContenido));
                contenido.Add(new Phrase($"Monto: {credito.ValorCredito.ToString("C",new System.Globalization.CultureInfo("es-CR"))}\n", fontContenido));
                contenido.Add(new Phrase($"Cliente: {credito.Cliente.Nombre}\n", fontContenido));
                document.Add(contenido);

                // Espacio adicional antes del versículo
                document.Add(new Paragraph(" ", fontContenido));

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

        public byte[] GenerarAbonoPdf(Abono abono)
        {
            using (var ms = new MemoryStream())
            {
                // Definir un tamaño de página personalizado de 80 mm de ancho
                var pageWidth = 80f * 2.83f; // 80 mm en puntos (aprox. 3.15 pulgadas)
                var pageSize = new Rectangle(pageWidth, PageSize.A7.Height); // Ajuste la altura según sea necesario
                var document = new Document(pageSize, 10, 10, 10, 10); // Márgenes de 10 puntos

                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Fuente para el contenido con tamaños reducidos
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontContenido = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                var fontVersiculo = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 7);

                // Título
                var titulo = new Paragraph("Comprobante de Abono", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15
                };
                document.Add(titulo);

                // Contenido del comprobante
                var contenido = new Paragraph
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 10
                };
                contenido.Add(new Phrase($"Descripción: {abono.Descripcion}\n", fontContenido));
                contenido.Add(new Phrase($"Fecha: {abono.FechaAbono:dd/MM/yyyy}\n", fontContenido));
                contenido.Add(new Phrase($"Monto: {abono.ValorAbono.ToString("C", new System.Globalization.CultureInfo("es-CR"))}\n", fontContenido));
                contenido.Add(new Phrase($"Cliente: {abono.Cliente.Nombre}\n", fontContenido));
                document.Add(contenido);

                // Espacio adicional antes del versículo
                document.Add(new Paragraph(" ", fontContenido));

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

        public byte[] GenerarGasolinaPdf(Gasolina gasolina)
        {
            using (var ms = new MemoryStream())
            {
                // Definir un tamaño de página personalizado de 80 mm de ancho
                var pageWidth = 80f * 2.83f; // 80 mm en puntos (aprox. 3.15 pulgadas)
                var pageSize = new Rectangle(pageWidth, PageSize.A7.Height); // Ajuste la altura según sea necesario
                var document = new Document(pageSize, 10, 10, 10, 10); // Márgenes de 10 puntos

                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Fuente para el contenido con tamaños reducidos
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontContenido = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                var fontVersiculo = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 7);

                // Título
                var titulo = new Paragraph("Comprobante de Abono Gasolina", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15
                };
                document.Add(titulo);

                // Contenido del comprobante
                var contenido = new Paragraph
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 10
                };
                contenido.Add(new Phrase($"Descripción: {gasolina.Comentario}\n", fontContenido));
                contenido.Add(new Phrase($"Fecha: {gasolina.FechaOperacion:dd/MM/yyyy}\n", fontContenido));
                contenido.Add(new Phrase($"Precio: {gasolina.PrecioGalonCargado.ToString("C", new System.Globalization.CultureInfo("es-CR"))}\n", fontContenido));
                contenido.Add(new Phrase($"Cantidad: {gasolina.CantGalonCargado.ToString()}\n", fontContenido));
                contenido.Add(new Phrase($"Monto: {gasolina.TotalGalonCargado.ToString("C", new System.Globalization.CultureInfo("es-CR"))}\n", fontContenido));
                contenido.Add(new Phrase($"Cliente: {gasolina.Cliente.Nombre}\n", fontContenido));
                document.Add(contenido);

                // Espacio adicional antes del versículo
                document.Add(new Paragraph(" ", fontContenido));

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