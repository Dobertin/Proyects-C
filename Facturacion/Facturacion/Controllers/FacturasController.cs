using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class FacturasController : Controller
    {
        private readonly FacturaRepository _repository;
        private readonly ClienteRepository _clienteRepository;
        private readonly ProductoRepository _productoRepository;
        private readonly TipoImpuestoRepository _tipoImpuestoRepository;
        private readonly TipoPagoRepository _tipoPagoRepository;
        private readonly PromocionRepository _promocionRepository;

        public FacturasController(FacturaRepository repository, ClienteRepository clienteRepository, ProductoRepository productoRepository, TipoImpuestoRepository tipoImpuestoRepository, TipoPagoRepository tipoPagoRepository, PromocionRepository promocionRepository)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _productoRepository = productoRepository;
            _tipoImpuestoRepository = tipoImpuestoRepository;
            _tipoPagoRepository = tipoPagoRepository;
            _promocionRepository = promocionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var facturas = await _repository.GetAllAsync();
            return View(facturas);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = await _clienteRepository.GetAllAsync();
            ViewBag.Productos = await _productoRepository.GetAllAsync();
            ViewBag.TiposImpuestos = await _tipoImpuestoRepository.GetAllAsync();
            ViewBag.TiposPagos = await _tipoPagoRepository.GetAllAsync();
            ViewBag.Promociones = await _promocionRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FacturaDTO facturaDTO)
        {
            if (ModelState.IsValid)
            {
                var factura = new Factura
                {
                    ClienteID = facturaDTO.ClienteID,
                    FechaEmision = DateTime.Now,
                    Productos = facturaDTO.Productos.Where(p => p != null && p.ProductoID != 0 && p.Cantidad > 0).Select(p => new FacturaProducto
                    {
                        ProductoID = p.ProductoID,
                        Cantidad = p.Cantidad,
                        Precio = p.Precio
                    }).ToList(),
                    Impuestos = facturaDTO.Impuestos.Where(i => i != null && i.TipoImpuestoID != 0 && i.Monto > 0).Select(i => new FacturaImpuesto
                    {
                        TipoImpuestoID = i.TipoImpuestoID,
                        Monto = i.Monto
                    }).ToList(),
                    Descuentos = facturaDTO.Descuentos.Where(d => d != null && d.PromocionID != 0 && d.Monto > 0).Select(d => new FacturaDescuento
                    {
                        PromocionID = d.PromocionID,
                        Monto = d.Monto
                    }).ToList(),
                    MetodoPagoID = facturaDTO.MetodoPagoID,
                    Estado = facturaDTO.Estado
                };

                factura.Total = CalcularTotalFactura(factura);
                await _repository.AddAsync(factura);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clientes = await _clienteRepository.GetAllAsync();
            ViewBag.Productos = await _productoRepository.GetAllAsync();
            ViewBag.TiposImpuestos = await _tipoImpuestoRepository.GetAllAsync();
            ViewBag.TiposPagos = await _tipoPagoRepository.GetAllAsync();
            ViewBag.Promociones = await _promocionRepository.GetAllAsync();
            return View(facturaDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var factura = await _repository.GetByIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            var facturaDTO = new FacturaDTO
            {
                ID = factura.ID,
                ClienteID = factura.ClienteID,
                FechaEmision = factura.FechaEmision,
                Productos = factura.Productos.Select(p => new FacturaProductoDTO
                {
                    ProductoID = p.ProductoID,
                    Cantidad = p.Cantidad,
                    Precio = p.Precio
                }).ToList(),
                Impuestos = factura.Impuestos.Select(i => new FacturaImpuestoDTO
                {
                    TipoImpuestoID = i.TipoImpuestoID,
                    Monto = i.Monto
                }).ToList(),
                Descuentos = factura.Descuentos.Select(d => new FacturaDescuentoDTO
                {
                    PromocionID = d.PromocionID,
                    Monto = d.Monto
                }).ToList(),
                MetodoPagoID = factura.MetodoPagoID,
                Estado = factura.Estado
            };

            ViewBag.Clientes = await _clienteRepository.GetAllAsync();
            ViewBag.Productos = await _productoRepository.GetAllAsync();
            ViewBag.TiposImpuestos = await _tipoImpuestoRepository.GetAllAsync();
            ViewBag.TiposPagos = await _tipoPagoRepository.GetAllAsync();
            ViewBag.Promociones = await _promocionRepository.GetAllAsync();
            return View(facturaDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FacturaDTO facturaDTO)
        {
            if (id != facturaDTO.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var factura = new Factura
                {
                    ID = facturaDTO.ID,
                    ClienteID = facturaDTO.ClienteID,
                    FechaEmision = facturaDTO.FechaEmision,
                    Productos = facturaDTO.Productos.Where(p => p != null && p.ProductoID != 0 && p.Cantidad > 0).Select(p => new FacturaProducto
                    {
                        ProductoID = p.ProductoID,
                        Cantidad = p.Cantidad,
                        Precio = p.Precio
                    }).ToList(),
                    Impuestos = facturaDTO.Impuestos.Where(i => i != null && i.TipoImpuestoID != 0 && i.Monto > 0).Select(i => new FacturaImpuesto
                    {
                        TipoImpuestoID = i.TipoImpuestoID,
                        Monto = i.Monto
                    }).ToList(),
                    Descuentos = facturaDTO.Descuentos.Where(d => d != null && d.PromocionID != 0 && d.Monto > 0).Select(d => new FacturaDescuento
                    {
                        PromocionID = d.PromocionID,
                        Monto = d.Monto
                    }).ToList(),
                    MetodoPagoID = facturaDTO.MetodoPagoID,
                    Estado = facturaDTO.Estado
                };

                factura.Total = CalcularTotalFactura(factura);
                await _repository.UpdateAsync(id, factura);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clientes = await _clienteRepository.GetAllAsync();
            ViewBag.Productos = await _productoRepository.GetAllAsync();
            ViewBag.TiposImpuestos = await _tipoImpuestoRepository.GetAllAsync();
            ViewBag.TiposPagos = await _tipoPagoRepository.GetAllAsync();
            ViewBag.Promociones = await _promocionRepository.GetAllAsync();
            return View(facturaDTO);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _repository.GetByIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private decimal CalcularTotalFactura(Factura factura)
        {
            var totalProductos = factura.Productos.Sum(p => p.Precio * p.Cantidad);
            var totalImpuestos = factura.Impuestos.Sum(i => i.Monto);
            var totalDescuentos = factura.Descuentos.Sum(d => d.Monto);

            return totalProductos + totalImpuestos - totalDescuentos;
        }
    }
}
