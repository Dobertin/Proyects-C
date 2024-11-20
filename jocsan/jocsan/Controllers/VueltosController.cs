using jocsan.Models;
using jocsan.Models.results;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class VueltosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VueltosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Vueltos()
        {
            return View();
        }

        [HttpGet("/Vueltos/vuelto/{idcliente}")]
        public async Task<IActionResult> ObtenerDatosVueltosAsync(int idcliente)
        {
            var vuelto = await _unitOfWork.Vuelto.ObtenerVueltoPorClienteTotalAsync(idcliente);

            return Ok(vuelto);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarVueltoAsync(Vuelto vuelto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // Asignar FechaCreacion y UsuarioCreacion a la factura
                vuelto.FechaCreacion = DateTime.Now;
                vuelto.FechaVuelto = DateTime.Now;
                vuelto.UsuarioCreacion ??= "System";
                await _unitOfWork.Vuelto.AddAsync(vuelto);
                // Guardar todos los detalles de la factura
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Generación del PDF
                var pdfGenerator = new PdfGenerator();

                //Agregar el cliente y el producto a la clase principal
                vuelto.Cliente = await _unitOfWork.Cliente.GetbyIDAsync(vuelto.IdCliente);

                var pdfBytes = pdfGenerator.GenerarVueltoPdf(vuelto);

                //Retornar el PDF como respuesta
                var fileName = $"Vuelto_{vuelto.IdVuelto}.pdf";
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
