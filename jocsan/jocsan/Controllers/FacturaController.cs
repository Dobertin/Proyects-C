using jocsan.Data;
using jocsan.Models;
using jocsan.Models.querys;
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
        [HttpGet("/Factura/visualizar/{idfactura}")]
        public async Task<IActionResult> VisualizarFacturaAsync(int idfactura)
        {
            try
            {
                // Obtener la factura principal
                Factura factura = await _unitOfWork.Factura.GetByIdAsync(idfactura);
                if (factura == null)
                {
                    return NotFound("Factura no encontrada");
                }

                // Cargar los detalles de la factura asociados
                factura.DetalleFacturas = (await _unitOfWork.DetalleFactura.GetAllAsync())
                    .Where(df => df.IdFactura == idfactura)
                    .ToList();

                // Cargar el cliente y los productos en los detalles
                factura.Cliente = await _unitOfWork.Cliente.GetByIdAsync(factura.IdCliente);

                var productosIds = factura.DetalleFacturas.Select(df => df.IdProducto).Distinct();
                var productos = (await _unitOfWork.Producto.GetAllAsync())
                    .Where(p => productosIds.Contains(p.IdProducto))
                    .ToDictionary(p => p.IdProducto);

                foreach (var detalle in factura.DetalleFacturas)
                {
                    detalle.Producto = productos[detalle.IdProducto];
                }

                // Generar el PDF de la factura
                var pdfGenerator = new PdfGenerator();
                var pdfBytes = pdfGenerator.GenerarFacturaPdf(factura);

                // Retornar el PDF como respuesta
                var fileName = $"Factura_{factura.IdFactura}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar la factura: {ex.Message}");
            }
        }
        [HttpDelete("/Factura/eliminar/{idfactura}")]
        public async Task<IActionResult> EliminarFacturaAsync(int idfactura)
        {
            try
            {
                // Actualizar Factura en vez de eliminar
                await _unitOfWork.Factura.EliminarFacturaAsync(idfactura);
                return Ok("Factura eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la factura: {ex.Message}");
            }
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
                if (factura.DetalleFacturas != null)
                {
                    foreach (var detalle in factura.DetalleFacturas)
                    {
                        detalle.FechaCreacion = DateTime.Now; // Asigna la fecha de creación
                        detalle.UsuarioCreacion ??= "System";
                    }
                }

                // Guardar todos los detalles de la factura
                await _unitOfWork.Factura.AddAsync(factura);
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

        [HttpPost]
        public async Task<IActionResult> ObtenerFacturasAsync(FacturaQuery facturaQuery)
        {
            var listadoFactura = await _unitOfWork.Factura.ObtenerFacturasAsync(facturaQuery);
            return Ok(listadoFactura);
        }
    }

}
