using jocsan.Data;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CuentasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult ConsolidadoCuentas()
        {
            return View();
        }
    }
}
