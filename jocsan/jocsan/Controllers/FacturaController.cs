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
        [HttpGet("/Factura/ultimonumero")]
        public async Task<IActionResult> ObtenerUltimaFacturaAsync()
        {
            var numero = await _unitOfWork.Factura.ObtenerUltimoNumeroFacturaAsync();
            return Ok(numero);
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
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // Asignar FechaCreacion y UsuarioCreacion a la factura
                factura.FechaCreacion = DateTime.Now;
                factura.FechaVenta = DateTime.Now;
                factura.UsuarioCreacion ??= "System"; // Cambia "System" por el usuario actual si aplica
                                                      // Asegúrate de que cada DetalleFactura tenga FechaCreacion asignada
                if (factura.DetalleFacturas != null)
                {
                    foreach (var detalle in factura.DetalleFacturas)
                    {
                        detalle.FechaCreacion = DateTime.Now; // Asigna la fecha de creación
                        detalle.UsuarioCreacion ??= "System";
                    }
                }

                // Agregar solo la factura para obtener el IdFactura
                await _unitOfWork.Factura.AddAsync(factura);
                await _unitOfWork.SaveChangesAsync(); 

                // Guardar todos los detalles de la factura
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Generación del PDF
                var pdfGenerator = new PdfGenerator();

                //Agregar el cliente y el producto a la clase principal
                factura.Cliente = await _unitOfWork.Cliente.GetByIdAsync(factura.IdCliente);
                foreach (var detalle in factura.DetalleFacturas)
                {
                    detalle.Producto = await _unitOfWork.Producto.GetByIdAsync(detalle.IdProducto);
                }
                var pdfBytes = pdfGenerator.GenerarFacturaPdf(factura);

                //Retornar el PDF como respuesta
                var fileName = $"Factura_{factura.IdFactura}.pdf";
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
