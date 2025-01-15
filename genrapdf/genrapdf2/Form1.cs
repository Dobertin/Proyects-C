using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.VisualBasic.Logging;

namespace genrapdf2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ruta del archivo PDF generado
            string outputPath = "Carta_Preaprobacion.pdf";

            // Crear un documento PDF
            Document document = new Document(PageSize.A4, 50, 50, 80, 80);

            // Crear un archivo PDF
            using (var stream = new FileStream(outputPath, FileMode.Create))
            {
                //PdfWriter.GetInstance(document, stream);
                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                // Ruta del logo
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "logoFondosColectivos.jpg");
                iTextSharp.text.Image logo_jpg = iTextSharp.text.Image.GetInstance(imagePath);
                logo_jpg.ScaleAbsolute(160f, 50f);

                // Configurar el evento de cabecera
                HeaderFooterEvent headerFooter = new HeaderFooterEvent(logo_jpg);
                writer.PageEvent = headerFooter; // Activar la cabecera

                document.Open();

                // Fuentes para el PDF
                var titleFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD);
                var normalFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL);
                var boldFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD);
                var normal10 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
                var normal09 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);

                // Variables dinámicas
                string fecha = "XX de XXXXX del XXXX";
                string nombreConcesionario = "XXXXXXXXXXXXXX";
                string nombreAsesor = "XXXXXXXXXXXXXX";
                string nombreCliente = "XXXXXXXXXXXXXX";
                string dniCliente = "XXXXXXXXXXXXXX";
                string correoCliente = "XXXXXXXXXXXXXX";
                string celularCliente = "XXXXXXXXXXXXXX";
                string operacionNumero = "XXXXXXXXXXXXXX";
                string marcaVehiculo = "XXXXXXXXXXXXXX";
                string modeloVehiculo = "XXXXXXXXXXXXXX";
                string programa = "XXXXXXXXXXXXXX";
                string tipoPrograma = "XXXXXXXXXXXXXX";
                string grupo = "XXXXXXXXXXXXXX";
                string plazo = "XXXXXXXXXXXXXX";
                string valorVehiculo = "XXXXXXXXXXXXXX";
                string cuotaInicial = "XXXXXXXXXXXXXX";
                string certificadoCompra = "XXXXXXXXXXXXXX";
                string nombreJefeCredito = "XXXXXXXXXXXXXX";
                string nombreCoordinadoraOP = "XXXXXXXXXXXXXX";

                // Título
                Paragraph title = new Paragraph("CARTA DE PREAPROBACIÓN DE CERTIFICADO DE COMPRA ADJUDICADO", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph("\n"));

                // Fecha y destinatario
                Paragraph paragraph = new Paragraph($"Lima, {fecha}", normalFont);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Señores:", normalFont));
                document.Add(new Paragraph($"{nombreConcesionario}", boldFont));
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph("Presente,", normalFont));
                document.Add(new Paragraph($"Atención: Sr. (a): {nombreAsesor}", normalFont));
                document.Add(new Paragraph("\n"));

                // Introducción
                document.Add(new Paragraph("De nuestra consideración:", normalFont));

                paragraph = new Paragraph();
                paragraph.Add(new Chunk("\nEs grato dirigirnos a usted para informarle la ", normalFont));
                paragraph.Add(new Chunk("preaprobación", boldFont));
                paragraph.Add(new Chunk(" de Compra de Certificado, de acuerdo al siguiente detalle:", normalFont));
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));

                // Detalle
                PdfPTable table = new PdfPTable(2) { WidthPercentage = 80 };
                table.SetWidths(new float[] { 30f, 70f }); // Ajusta las proporciones de las columnas
                table.AddCell(new PdfPCell(new Phrase("Nombre del cliente", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(nombreCliente, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("DNI", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(dniCliente, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("N° de Operación", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(operacionNumero, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Marca del vehículo", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(marcaVehiculo, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Modelo del vehículo", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(modeloVehiculo, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Programa", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(programa, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Tipo de Programa", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(tipoPrograma, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Grupo", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(grupo, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Plazo", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(plazo, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Valor del vehículo", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(valorVehiculo, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Cuota inicial", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(cuotaInicial, normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Certificado de Compra", normalFont)));
                table.AddCell(new PdfPCell(new Phrase(certificadoCompra, normalFont)));
                document.Add(table);


                paragraph = new Paragraph($"\nCon la finalidad de iniciar el proceso de aprobación de su compra certificado adjudicado, agradeceremos entregar a nuestro " +
                    $"funcionario el  Sr. (a) {nombreCliente} con celular {celularCliente} y correo electrónico {correoCliente} la " +
                    $"documentación que le solicitaremos por correo electrónico, de acuerdo a los siguientes criterios: ", normalFont);
                paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));

                paragraph = new Paragraph($"• Acreditación de identidad", normalFont);
                paragraph.IndentationLeft = 20f;
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));

                table = new PdfPTable(2) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 55f, 45f }); // Ajusta las proporciones de las columnas
                PdfPCell pdfcell = new PdfPCell(new Phrase("Criterios", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                pdfcell = new PdfPCell(new Phrase("Documento", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Persona Nacional", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("DNI", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Persona Nacional casado", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Partida de Matrimonio", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Persona Nacional divorciado, pero su DNI indica casado", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Certificado de divorcio", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Persona Nacional casado con separación de bienes", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Inscripción de separación de patrimonio", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Persona Extranjera", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Carné de extranjería", normalFont)));
                document.Add(table);
                document.Add(new Paragraph("\n"));

                paragraph = new Paragraph($"• Acreditación de ingresos: Retail", normalFont);
                paragraph.IndentationLeft = 20f;
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));

                // Crear tabla con dos columnas
                table = new PdfPTable(2) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 20f, 80f }); // Ajusta las proporciones de las columnas

                // Encabezados de la tabla
                pdfcell = new PdfPCell(new Phrase("Categoría Laboral", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                pdfcell = new PdfPCell(new Phrase("Documento", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                // Primera fila
                pdfcell = new PdfPCell(new Phrase("1ra Categoría", normalFont));
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Rowspan = 3; // Combinar filas
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Reporte Tributario", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Reporte electrónico de declaraciones y pagos de los últimos 12 meses.", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("HR y PU", normalFont)));

                // Segunda fila
                pdfcell = new PdfPCell(new Phrase("4ta Categoría", normalFont));
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Rowspan = 2; // Combinar filas
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Reporte Tributario", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Reporte electrónico de declaraciones y pagos de los últimos 12 meses.", normalFont)));

                // Tercera fila
                pdfcell = new PdfPCell(new Phrase("5ta Categoría", normalFont));
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Rowspan = 2; // Combinar filas
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Boleta", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Reporte Tributario", normalFont)));

                // Agregar la tabla al documento
                document.Add(table);

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Considerar la siguiente sección solo si la solicitud corresponde a una persona jurídica:", normalFont));
                document.Add(new Paragraph("\n"));

                paragraph = new Paragraph($"• Acreditación de ingresos: Negocios ", normalFont);
                paragraph.IndentationLeft = 20f;
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));

                table = new PdfPTable(1) { WidthPercentage = 100 };
                pdfcell = new PdfPCell(new Phrase("Negocios | Emprendedor", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Ficha Cliente", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Copia del último recibo de servicios", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Declaración jurada de ingresos", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Boleta de compra (3 últimas)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Fotos del negocio (5)*", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Estado de cuenta del cliente", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Contrato de alquiler", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Tarjeta de propiedad**", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Cronograma de deuda (si la deuda MYPE > s/ 20,000)", normalFont)));
                document.Add(table);

                document.Add(new Paragraph("*Se debe visualizar mercadería, maquinaria, fachada, dirección del negocio, medidor de luz del inmueble, cliente.", normal09));
                document.Add(new Paragraph("**En caso que el giro del negocio sea transporte.", normal09));
                document.Add(new Paragraph("\n"));

                table = new PdfPTable(1) { WidthPercentage = 100 };
                pdfcell = new PdfPCell(new Phrase("Negocios | Régimen General o RMT", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Reporte Tributario (máx. 8 días de antigüedad)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Reporte electrónico de declaraciones y pagos de los últimos 12 meses", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Ficha Cliente", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Ficha RUC formato SUNAT (máx. 8 días de antigüedad)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Última DJ anual original en formato SUNAT", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Declaración jurada de ingresos*", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Cronograma de deuda (si la deuda MYPE > s/ 20,000)", normalFont)));
                document.Add(table);

                document.Add(new Paragraph("*Si utilidad < S/20M", normal09));
                document.Add(new Paragraph("\n"));

                table = new PdfPTable(1) { WidthPercentage = 100 };
                pdfcell = new PdfPCell(new Phrase("Negocios | Régimen Especial o RUS", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Reporte Tributario (máx. 8 días de antigüedad)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Reporte electrónico de declaraciones y pagos de los últimos 12 meses", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Ficha Cliente", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Ficha RUC formato SUNAT (máx. 8 días de antigüedad)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Copia del último recibo de servicios (del domicilio, RRLL o accionista)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Boleta de compra (3 últimas)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Boleta de venta (3 últimas)", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Fotos del negocio (5)*", normalFont)));
                table.AddCell(new PdfPCell(new Phrase("Cronograma de deuda (si la deuda MYPE > s/ 20,000)", normalFont)));
                document.Add(table);

                document.Add(new Paragraph("*Se debe visualizar mercadería, maquinaria, fachada, dirección del negocio, medidor de luz del inmueble, cliente.", normal09));
                document.Add(new Paragraph("\n"));

                paragraph = new Paragraph($"• Acreditación domiciliaria", normalFont);
                paragraph.IndentationLeft = 20f;
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Debe presentar un documento que acredite su domicilio, entre ellos:", normalFont));
                document.Add(new Paragraph("\n"));

                table = new PdfPTable(2) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 50f, 50f }); // Ajusta las proporciones de las columnas
                pdfcell = new PdfPCell(new Phrase("Servicio Básico", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                pdfcell = new PdfPCell(new Phrase("Exigencia", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);

                table.AddCell(new PdfPCell(new Phrase("Agua", normalFont)));
                pdfcell = new PdfPCell(new Phrase("Último recibo", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                table.AddCell(new PdfPCell(new Phrase("Energía eléctrica", normalFont)));
                pdfcell = new PdfPCell(new Phrase("Último recibo", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                table.AddCell(new PdfPCell(new Phrase("Gas", normalFont)));
                pdfcell = new PdfPCell(new Phrase("Último recibo", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                table.AddCell(new PdfPCell(new Phrase("Internet", normalFont)));
                pdfcell = new PdfPCell(new Phrase("Último recibo", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                table.AddCell(new PdfPCell(new Phrase("Teléfono fijo", normalFont)));
                pdfcell = new PdfPCell(new Phrase("Último recibo", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(pdfcell);
                document.Add(table);
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("La documentación de acreditación podrá variar dependiendo de la categoría de renta de ingresos declarados y estado civil. " +
                    "Es requisito indispensable, para realizar la aprobación y posterior desembolso del certificado de compra adjudicado, la verificación laboral y domiciliaria", normalFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("En caso de que en el proceso de acreditación se detecte información " +
                    "y/o documentación inconsistente con lo declarado por el asociado, la presente carta quedará sin efecto.", normalFont));
                document.Add(new Paragraph("\n"));

                // Despedida
                document.Add(new Paragraph("Atentamente,", normalFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));
                table = new PdfPTable(2) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 50f, 50f }); // Ajusta las proporciones de las columnas
                pdfcell = new PdfPCell(new Phrase("___________________________", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
                table.AddCell(pdfcell);

                pdfcell = new PdfPCell(new Phrase("___________________________", boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
                table.AddCell(pdfcell);

                pdfcell = new PdfPCell(new Phrase(nombreJefeCredito, boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
                table.AddCell(pdfcell);

                pdfcell = new PdfPCell(new Phrase(nombreCoordinadoraOP, boldFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
                table.AddCell(pdfcell);

                pdfcell = new PdfPCell(new Phrase("Jefe de Créditos", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
                table.AddCell(pdfcell);

                pdfcell = new PdfPCell(new Phrase("Coordinadora de Operaciones", normalFont));
                pdfcell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfcell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
                table.AddCell(pdfcell);
                document.Add(table);

                document.Close();
            }
            MessageBox.Show("Proceso terminado");
        }
    }
    public class HeaderFooterEvent : PdfPageEventHelper
    {
        private iTextSharp.text.Image _logo;

        public HeaderFooterEvent(iTextSharp.text.Image logo)
        {
            _logo = logo;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Crear una tabla para la cabecera con una columna
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; // Establecer ancho total
            headerTable.LockedWidth = true; // Bloquear el ancho de la tabla

            // Agregar la imagen como cabecera
            PdfPCell cell = new PdfPCell(_logo, false);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
            headerTable.AddCell(cell);

            // Posicionar la tabla de cabecera
            headerTable.WriteSelectedRows(
                0, -1, // Todas las filas
                document.LeftMargin, // Coordenada X
                document.PageSize.Height - 30, // Coordenada Y
                writer.DirectContent
            );

            // Crear una tabla para el footer con una columna
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.LockedWidth = true;
            //footerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            // Definir fuente para el texto del footer
            var fuente = FontFactory.GetFont("Arial Narrow", 8, iTextSharp.text.Font.NORMAL);

            // Agregar el texto del footer
            PdfPCell footerCell = new PdfPCell(new Phrase("Santander Empresa Administradora de Fondos Colectivos S.A. - Santander EAFC S.A. autorizada a funcionar por la SMV" +
                "\nmediante Resolución de Superintendente Nº 140-2024-SMV/02" +
                "\nwww.santanderfondoscolectivos.com.pe" +
                "\nTelf. (01) 480 - 1414", fuente));

            footerCell.HorizontalAlignment = Element.ALIGN_CENTER; // Centrar el texto
            footerCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
            footerTable.AddCell(footerCell);

            footerCell = new PdfPCell(new Phrase("FC012401001", fuente));
            footerCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // Sin bordes
            footerTable.AddCell(footerCell);

            // Calcular la posición X y centrar el footer
            float footerXPosition = (document.PageSize.Width - footerTable.TotalWidth) / 2;

            // Posicionar el footer
            footerTable.WriteSelectedRows(
                0, -1, // Todas las filas
                footerXPosition, // Coordenada X centrada
                document.BottomMargin - 10, // Coordenada Y (altura desde la parte inferior)
                writer.DirectContent
            );
        }

    }
}
