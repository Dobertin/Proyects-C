using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class TiposPagosController : Controller
    {
        private readonly TipoPagoRepository _repository;

        public TiposPagosController(TipoPagoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var tiposPagos = await _repository.GetAllAsync();
            return View(tiposPagos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TipoPago tipoPago)
        {
            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(tipoPago);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoPago);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tipoPago = await _repository.GetByIdAsync(id);
            if (tipoPago == null)
            {
                return NotFound();
            }
            return View(tipoPago);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TipoPago tipoPago)
        {
            if (id != tipoPago.ID)
            {
                return BadRequest();
            }

            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(id, tipoPago);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoPago);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tipoPago = await _repository.GetByIdAsync(id);
            if (tipoPago == null)
            {
                return NotFound();
            }
            return View(tipoPago);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
