using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class GasolinaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GasolinaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Gasolina()
        {
            return View();
        }
        [HttpGet("/Gasolina/gasolina/listar")]
        public async Task<IActionResult> ObtenerClientesAsync()
        {
            var listadocliente = await _unitOfWork.Cliente.GetComboClientesAsync();
            return Ok(listadocliente);
        }
        [HttpGet("/Gasolina/gasolina/{idcliente}")]
        public async Task<IActionResult> ObtenerDatosGasolinaPorIDAsync(int idcliente)
        {
            var data = await _unitOfWork.Gasolina.ObtenerDatosGasolinaPorID(idcliente);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarGasolinaCargadoAsync(Gasolina gasolina)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Gasolina.AddAsync(gasolina);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Generación del PDF
                var pdfGenerator = new PdfGenerator();

                //Agregar el cliente y el producto a la clase principal
                gasolina.Cliente = await _unitOfWork.Cliente.GetbyIDAsync(gasolina.IdCliente);
                var pdfBytes = pdfGenerator.GenerarGasolinaPdf(gasolina);

                //Retornar el PDF como respuesta
                var fileName = $"Gasolina_{gasolina.IdGasolina}.pdf";
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
