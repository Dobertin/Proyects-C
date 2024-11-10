using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using jocsan.Repository.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClienteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult AniadirCliente()
        {
            return View();
        }
        public IActionResult ListadoClientes()
        {
            return View();
        }
        [HttpGet("/Clientes/clientes/listar")]
        public async Task<IActionResult> ObtenerClientesAsync()
        {
            var listadocliente = await _unitOfWork.Cliente.ObtenerInformacionClientesAsync();
            return Ok(listadocliente);
        }
        [HttpGet("/Clientes/clientes/{idCliente}")]
        public async Task<IActionResult> ObtenerClientesporIDAsync(int idCliente)
        {
            var cliente = await _unitOfWork.Cliente.GetbyIDAsync(idCliente);
            return Ok(cliente);
        }
        [HttpDelete("/Clientes/eliminar/{idcliente}")]
        public async Task<IActionResult> EliminarCreditosAsync(int idcliente)
        {
            try
            {
                // Actualizar Factura en vez de eliminar
                await _unitOfWork.Cliente.EliminarClienteAsync(idcliente);
                return Ok("Cliente eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la factura: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddClienteAsync(Cliente cliente)
        {
            try
            {
                // Asignar valores de auditoría si aplican
                cliente.FechaCreacion = DateTime.Now;
                cliente.Estado = 1;
                cliente.UsuarioCreacion ??= "System"; // Asignar el usuario actual si corresponde

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Cliente.AddAsync(cliente);
                await _unitOfWork.CommitTransactionAsync();
                return Ok("Cliente Agregado Correctamente");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest("Error" + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarAsync(Cliente cliente)
        {
            try
            {
                // Asignar valores de auditoría si aplican
                cliente.FechaModifica = DateTime.Now;
                cliente.Estado = 1;
                cliente.UsuarioModifica ??= "System"; // Asignar el usuario actual si corresponde

                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Cliente.Update(cliente);
                await _unitOfWork.CommitTransactionAsync();
                return Ok("");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest("Error" + ex.Message);
            }
        }

    }
}
