using Sistema.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Negocio.Services
{
    public class TiendaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TiendaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
