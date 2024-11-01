using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FacturaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Emitir()
        {
            return View();
        }
        public IActionResult BuscarFactura()
        {
            return View();
        }
        [HttpGet("/Factura/clientes/listar")]
        public async Task<IActionResult> ObtenerClientesAsync()
        {
            var listadocliente = await _unitOfWork.Cliente.GetComboClientesAsync();
            return Ok(listadocliente);
        }
        [HttpGet("/Factura/Parametro/porcentajes")]
        public async Task<IActionResult> ObtenerPorcentajesAsync()
        {
            var listadoPorcentaje = await _unitOfWork.Parametro.GetObtenerPorcentajesAsync();
            return Ok(listadoPorcentaje);
        }
        [HttpGet("/Factura/Parametro/preciohielo")]
        public async Task<IActionResult> ObtenerPreciohieloAsync()
        {
            var listadoPorcentaje = await _unitOfWork.Parametro.GetObtenerPreciohieloAsync();
            return Ok(listadoPorcentaje);
        }
        [HttpGet("/Factura/producto/listar")]
        public async Task<IActionResult> ObtenerProductosAsync()
        {
            var listadoProductos = await _unitOfWork.Producto.GetComboProductosAsync();
            return Ok(listadoProductos);
        }
        [HttpGet("/Factura/clientes/{idcliente}")]
        public async Task<IActionResult> ObtenerDatosClienteAsync(int idcliente)
        {
            var cliente = await _unitOfWork.Cliente.GetByIdAsync(idcliente);
            return Ok(cliente);
        }
        [HttpGet("/Factura/producto/{idproducto}")]
        public async Task<IActionResult> ObtenerDatosProductoAsync(int idproducto)
        {
            var producto = await _unitOfWork.Producto.GetByIdAsync(idproducto);
            return Ok(producto);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarFacturaAsync(Factura factura)
        {
            var producto = await _unitOfWork.Producto.GetByIdAsync(1);
            return Ok(producto);
        }
    }

}
