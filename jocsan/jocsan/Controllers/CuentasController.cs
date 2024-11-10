using jocsan.Data;
using jocsan.Models;
using jocsan.Models.results;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CuentasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult ConsolidadoCuentas()
        {
            return View();
        }

        [HttpGet("/Cuentas/clientes/listar")]
        public async Task<IActionResult> ObtenerClientesAsync()
        {
            var listadocliente = await _unitOfWork.Cliente.GetComboClientesAsync();
            return Ok(listadocliente);
        }
        [HttpGet("/Cuentas/cuentas/{idcliente}")]
        public async Task<IActionResult> ObtenerDatosCuentasAsync(int idcliente)
        {
            var creditos = await _unitOfWork.Creditos.ObtenerCreditosPorClienteTotalAsync(idcliente);
            var abonos = await _unitOfWork.Abono.ObtenerAbonoPorClienteTotalAsync(idcliente);
            var ultimacuenta = await _unitOfWork.Abono.ObtenerUltimoAbonoPorClienteAsync(idcliente);
            var facturas = await _unitOfWork.Factura.ObtenerFacturasPorClienteAsync(idcliente);

            CuentaResultTotal resultTotal = new CuentaResultTotal();
            resultTotal.CreditoTotal = creditos;
            resultTotal.AbonoTotal = abonos;
            resultTotal.FacturasTotal = facturas;
            resultTotal.UltimaCuenta = ultimacuenta;

            return Ok(resultTotal);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarAbonoAsync(Abono abono)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // Asignar FechaCreacion y UsuarioCreacion a la factura
                abono.FechaCreacion = DateTime.Now;
                abono.FechaAbono = DateTime.Now;
                abono.UsuarioCreacion ??= "System";
                await _unitOfWork.Abono.AddAsync(abono);
                // Guardar todos los detalles de la factura
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Generación del PDF
                var pdfGenerator = new PdfGenerator();

                //Agregar el cliente y el producto a la clase principal
                abono.Cliente = await _unitOfWork.Cliente.GetbyIDAsync(abono.IdCliente);

                var pdfBytes = pdfGenerator.GenerarAbonoPdf(abono);

                //Retornar el PDF como respuesta
                var fileName = $"Abono_{abono.IdAbono}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest("Error: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerDocumentoAsync(int id, string tipodoc)
        {
            try
            {
                var pdfGenerator = new PdfGenerator();
                byte[] pdfBytes = tipodoc switch
                {
                    "credito" => await GenerarCreditoPdf(id, pdfGenerator),
                    "abono" => await GenerarAbonoPdf(id, pdfGenerator),
                    "factura" => await GenerarFacturaPdf(id, pdfGenerator),
                    _ => throw new ArgumentException("Tipo de documento no válido")
                };

                var fileName = $"Cliente_{id}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest("Error: " + ex.Message);
            }
        }

        private async Task<byte[]> GenerarCreditoPdf(int id, PdfGenerator pdfGenerator)
        {
            var credito = await _unitOfWork.Creditos.GetbyIDAsync(id);
            credito.Cliente ??= await _unitOfWork.Cliente.GetbyIDAsync(credito.IdCliente);
            return pdfGenerator.GenerarCreditoPdf(credito);
        }

        private async Task<byte[]> GenerarAbonoPdf(int id, PdfGenerator pdfGenerator)
        {
            var abono = await _unitOfWork.Abono.GetbyIDAsync(id);
            abono.Cliente ??= await _unitOfWork.Cliente.GetbyIDAsync(abono.IdCliente);
            return pdfGenerator.GenerarAbonoPdf(abono);
        }

        private async Task<byte[]> GenerarFacturaPdf(int id, PdfGenerator pdfGenerator)
        {
            var factura = await _unitOfWork.Factura.GetbyIDAsync(id);
            if (factura == null)
            {
                throw new KeyNotFoundException("Factura no encontrada");
            }

            factura.DetalleFacturas = (await _unitOfWork.DetalleFactura.GetAllAsync())
                .Where(df => df.IdFactura == id)
                .ToList();

            factura.Cliente = await _unitOfWork.Cliente.GetbyIDAsync(factura.IdCliente);

            var productosIds = factura.DetalleFacturas.Select(df => df.IdProducto).Distinct();
            var productos = (await _unitOfWork.Producto.GetAllAsync())
                .Where(p => productosIds.Contains(p.IdProducto))
                .ToDictionary(p => p.IdProducto);

            foreach (var detalle in factura.DetalleFacturas)
            {
                detalle.Producto = productos[detalle.IdProducto];
            }

            return pdfGenerator.GenerarFacturaPdf(factura);
        }

    }
}
