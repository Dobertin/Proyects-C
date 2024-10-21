using jocsan.Data;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FacturaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Emitir()
        {
            return View();
        }
        public IActionResult BuscarFactura()
        {
            return View();
        }
    }

}
