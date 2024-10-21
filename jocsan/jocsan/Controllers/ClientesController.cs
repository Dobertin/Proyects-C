using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using jocsan.Repository.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult AñadirCliente()
        {
            return View();
        }
        public IActionResult ListadoClientes()
        {
            return View();
        }

        // GET: Clientes/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        //Crear un cliente dentro de una transacción
        public async Task<IActionResult> CreateClienteWithCredito(Cliente cliente, Credito credito)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Crear el cliente
                await _unitOfWork.Clientes.AddAsync(cliente);

                // Asociar el cliente con el crédito
                credito.IdCliente = cliente.IdCliente;
                await _unitOfWork.Credito.AddAsync(credito);

                // Confirmar la transacción
                await _unitOfWork.CommitTransactionAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Si algo falla, hacer rollback de la transacción
                await _unitOfWork.RollbackTransactionAsync();
                return View("Error");
            }
        }
    }
}
