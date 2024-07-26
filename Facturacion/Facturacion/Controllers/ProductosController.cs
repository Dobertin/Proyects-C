using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepository _productoRepository;
        private readonly PromocionRepository _promocionRepository;

        public ProductosController(ProductoRepository productoRepository, PromocionRepository promocionRepository)
        {
            _productoRepository = productoRepository;
            _promocionRepository = promocionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoRepository.GetAllAsync();
            var promociones = await _promocionRepository.GetAllAsync();

            var productosConPromocion = new Dictionary<int, bool>();

            foreach (var producto in productos)
            {
                productosConPromocion[producto.ID] = promociones.Any(p => p.ProductoIDs.Contains(producto.ID));
            }

            ViewBag.ProductosConPromocion = productosConPromocion;
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                producto.FechaCreacion = DateTime.Now;
                await _productoRepository.AddAsync(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.ID)
            {
                return BadRequest();
            }

            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _productoRepository.UpdateAsync(id, producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AsignarCategoria(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarCategoria(int id, string categoria)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            producto.Categoria = categoria;
            await _productoRepository.UpdateAsync(id, producto);
            return RedirectToAction(nameof(Index));
        }
    }
}
