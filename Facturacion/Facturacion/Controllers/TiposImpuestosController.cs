using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class TiposImpuestosController : Controller
    {
        private readonly TipoImpuestoRepository _repository;

        public TiposImpuestosController(TipoImpuestoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var tiposImpuestos = await _repository.GetAllAsync();
            return View(tiposImpuestos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TipoImpuesto tipoImpuesto)
        {
            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(tipoImpuesto);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoImpuesto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tipoImpuesto = await _repository.GetByIdAsync(id);
            if (tipoImpuesto == null)
            {
                return NotFound();
            }
            return View(tipoImpuesto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TipoImpuesto tipoImpuesto)
        {
            if (id != tipoImpuesto.ID)
            {
                return BadRequest();
            }

            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(id, tipoImpuesto);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoImpuesto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tipoImpuesto = await _repository.GetByIdAsync(id);
            if (tipoImpuesto == null)
            {
                return NotFound();
            }
            return View(tipoImpuesto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
