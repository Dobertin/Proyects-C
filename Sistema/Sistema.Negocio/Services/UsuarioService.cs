using Sistema.Database.Entities;
using Sistema.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Negocio.Services
{
    public class UsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
        {
            return await _unitOfWork.Usuario.GetAllAsync();
        }

        public async Task CambiarTiendaAsync(int idUsuario, int idTienda, int idAdmin)
        {
            // Lógica de validación antes de usar UnitOfWork
            var usuario = await _unitOfWork.Usuario.GetByIdAsync(idUsuario);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            usuario.IdTienda = idTienda;
            usuario.UsuarioActualizacion = idAdmin;

            _unitOfWork.Usuario.Update(usuario);
            await _unitOfWork.SaveAsync();
        }
    }

}
