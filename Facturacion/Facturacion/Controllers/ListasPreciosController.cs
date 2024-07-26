using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class ListasPreciosController : Controller
    {
        private readonly ListaPrecioRepository _repository;
        private readonly ProductoRepository _productoRepository;

        public ListasPreciosController(ListaPrecioRepository repository, ProductoRepository productoRepository)
        {
            _repository = repository;
            _productoRepository = productoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var listasPrecios = await _repository.GetAllAsync();
            return View(listasPrecios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ListaPrecio listaPrecio)
        {
            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(listaPrecio);
                return RedirectToAction(nameof(Index));
            }
            return View(listaPrecio);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var listaPrecio = await _repository.GetByIdAsync(id);
            if (listaPrecio == null)
            {
                return NotFound();
            }
            return View(listaPrecio);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ListaPrecio listaPrecio)
        {
            if (id != listaPrecio.ID)
            {
                return BadRequest();
            }

            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(id, listaPrecio);
                return RedirectToAction(nameof(Index));
            }
            return View(listaPrecio);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var listaPrecio = await _repository.GetByIdAsync(id);
            if (listaPrecio == null)
            {
                return NotFound();
            }
            return View(listaPrecio);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AsignarProductos(int id)
        {
            var listaPrecio = await _repository.GetByIdAsync(id);
            if (listaPrecio == null)
            {
                return NotFound();
            }

            var productos = await _productoRepository.GetAllAsync();
            ViewBag.Productos = productos;
            return View(listaPrecio);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarProductos(int id, List<int> productoIds, List<decimal> precios)
        {
            var listaPrecio = await _repository.GetByIdAsync(id);
            if (listaPrecio == null)
            {
                return NotFound();
            }

            listaPrecio.Productos = new List<PrecioProducto>();
            for (int i = 0; i < productoIds.Count; i++)
            {
                listaPrecio.Productos.Add(new PrecioProducto
                {
                    ProductoID = productoIds[i],
                    Precio = precios[i]
                });
            }

            await _repository.UpdateAsync(id, listaPrecio);
            return RedirectToAction(nameof(Index));
        }
    }
}
