using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using Rotativa.AspNetCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class ReportesController : Controller
    {
        private readonly FacturaRepository _facturaRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly ProductoRepository _productoRepository;

        public ReportesController(FacturaRepository facturaRepository, ClienteRepository clienteRepository, ProductoRepository productoRepository)
        {
            _facturaRepository = facturaRepository;
            _clienteRepository = clienteRepository;
            _productoRepository = productoRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> FacturasPorPeriodo(DateTime inicio, DateTime fin)
        {
            var facturas = await _facturaRepository.GetAllAsync();
            var facturasFiltradas = facturas.Where(f => f.FechaEmision >= inicio && f.FechaEmision <= fin).ToList();

            return new ViewAsPdf("FacturasPorPeriodo", facturasFiltradas)
            {
                FileName = "FacturasPorPeriodo.pdf"
            };
        }

        public async Task<IActionResult> VentasPorCliente(string clienteNombre)
        {
            var clientes = await _clienteRepository.GetAllAsync();
            var cliente = clientes.FirstOrDefault(c => c.Nombre.Equals(clienteNombre, StringComparison.OrdinalIgnoreCase));
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado");
            }

            var facturas = await _facturaRepository.GetAllAsync();
            var facturasCliente = facturas.Where(f => f.ClienteID == cliente.ID).ToList();

            ViewBag.ClienteNombre = cliente.Nombre;
            return new ViewAsPdf("VentasPorCliente", facturasCliente)
            {
                FileName = $"VentasPorCliente_{clienteNombre}.pdf"
            };
        }

        public async Task<IActionResult> VentasPorProducto(string productoNombre)
        {
            var productos = await _productoRepository.GetAllAsync();
            var producto = productos.FirstOrDefault(p => p.Nombre.Equals(productoNombre, StringComparison.OrdinalIgnoreCase));
            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            var facturas = await _facturaRepository.GetAllAsync();
            var facturasProducto = facturas.Where(f => f.Productos.Any(p => p.ProductoID == producto.ID)).ToList();

            // Obtener los nombres de los clientes
            var clientes = await _clienteRepository.GetAllAsync();
            var facturasProductoConClienteNombre = facturasProducto.Select(f => new
            {
                Factura = f,
                ClienteNombre = clientes.FirstOrDefault(c => c.ID == f.ClienteID)?.Nombre
            }).ToList();

            ViewBag.ProductoNombre = producto.Nombre;
            return new ViewAsPdf("VentasPorProducto", facturasProductoConClienteNombre)
            {
                FileName = $"VentasPorProducto_{productoNombre}.pdf"
            };
        }
    }
}
