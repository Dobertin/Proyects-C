using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class CreditoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreditoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Creditos()
        {
            return View();
        }
        [HttpGet("/Creditos/clientes/listar")]
        public async Task<IActionResult> ObtenerClientesAsync()
        {
            var listadocliente = await _unitOfWork.Cliente.GetComboClientesAsync();
            return Ok(listadocliente);
        }
        [HttpGet("/Creditos/listar/{idcliente}")]
        public async Task<IActionResult> ObtenerCreditosPorClienteAsync(int idcliente)
        {
            var listadocreditos = await _unitOfWork.Creditos.ObtenerCreditosPorClienteAsync(idcliente);
            return Ok(listadocreditos);
        }
        [HttpGet("/Creditos/visualizar/{idcredito}")]
        public async Task<IActionResult> VisualizarCreditosAsync(int idcredito)
        {
            try
            {
                // Obtener Credito principal
                Credito credito = await _unitOfWork.Creditos.GetbyIDAsync(idcredito);
                if (credito == null)
                {
                    return NotFound("Factura no encontrada");
                }

                credito.Cliente = await _unitOfWork.Cliente.GetbyIDAsync(credito.IdCliente);

                // Generar el PDF del Credito
                var pdfGenerator = new PdfGenerator();
                var pdfBytes = pdfGenerator.GenerarCreditoPdf(credito);

                // Retornar el PDF como respuesta
                var fileName = $"Credito_{credito.IdCredito}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar la factura: {ex.Message}");
            }
        }
        [HttpDelete("/Creditos/eliminar/{idcredito}")]
        public async Task<IActionResult> EliminarCreditosAsync(int idcredito)
        {
            try
            {
                // Actualizar Factura en vez de eliminar
                await _unitOfWork.Creditos.EliminarCreditoAsync(idcredito);
                return Ok("Factura eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la factura: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarCreditosAsync(Credito credito)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // Asignar FechaCreacion y UsuarioCreacion a la factura
                credito.FechaCreacion = DateTime.Now;
                credito.FechaCredito = DateTime.Now;
                credito.UsuarioCreacion ??= "System";
                await _unitOfWork.Creditos.AddAsync(credito);
                // Guardar todos los detalles de la factura
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Generación del PDF
                var pdfGenerator = new PdfGenerator();

                //Agregar el cliente y el producto a la clase principal
                credito.Cliente = await _unitOfWork.Cliente.GetbyIDAsync(credito.IdCliente);

                var pdfBytes = pdfGenerator.GenerarCreditoPdf(credito);

                //Retornar el PDF como respuesta
                var fileName = $"Credito_{credito.IdCredito}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
