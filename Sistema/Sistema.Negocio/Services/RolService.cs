using Sistema.Database.Entities;
using Sistema.Database.Interfaces;
using Sistema.Negocio.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Negocio.Services
{
    public class RolService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RolService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ComboGenericoDTO>> ObtenerComboRolAsync()
        {
            var lista = new List<ComboGenericoDTO>();
            var datos = await _unitOfWork.Rol.GetAllAsync();
            foreach (var dato in datos)
            {
                var combo = new ComboGenericoDTO();
                combo.Codigo = dato.IdRol;
                combo.Descripcion = dato.NombreRol;
                lista.Add(combo);
            }
            return lista;
        }
    }
}
