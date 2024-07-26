using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class PromocionesController : Controller
    {
        private readonly PromocionRepository _repository;
        private readonly ProductoRepository _productoRepository;

        public PromocionesController(PromocionRepository repository, ProductoRepository productoRepository)
        {
            _repository = repository;
            _productoRepository = productoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var promociones = await _repository.GetAllAsync();
            return View(promociones);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Promocion promocion)
        {
            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(promocion);
                return RedirectToAction(nameof(Index));
            }
            return View(promocion);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var promocion = await _repository.GetByIdAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }
            return View(promocion);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Promocion promocion)
        {
            if (id != promocion.ID)
            {
                return BadRequest();
            }

            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(id, promocion);
                return RedirectToAction(nameof(Index));
            }
            return View(promocion);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var promocion = await _repository.GetByIdAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }
            return View(promocion);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AsignarProductos(int id)
        {
            var promocion = await _repository.GetByIdAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }

            var productos = await _productoRepository.GetAllAsync();
            ViewBag.Productos = productos;
            return View(promocion);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarProductos(int id, List<int> productoIds)
        {
            var promocion = await _repository.GetByIdAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }

            promocion.ProductoIDs = productoIds;

            await _repository.UpdateAsync(id, promocion);
            return RedirectToAction(nameof(Index));
        }
    }
}
