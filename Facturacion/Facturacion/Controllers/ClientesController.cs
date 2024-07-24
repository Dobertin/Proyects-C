using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Threading.Tasks;

namespace Facturacion.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteRepository _repository;

        public ClientesController(ClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _repository.GetAllAsync();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                cliente.FechaRegistro = DateTime.Now;
                await _repository.AddAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.ID)
            {
                return BadRequest();
            }

            ModelState.Remove("_id"); // Remover la validación del campo _id
            ModelState.Remove("ID");  // Remover la validación del campo ID
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(id, cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string nombre, string direccion, string telefono, string email)
        {
            var clientes = await _repository.SearchAsync(nombre, direccion, telefono, email);
            return View("Index", clientes);
        }
    }
}
