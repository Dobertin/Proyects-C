using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class GasolinaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GasolinaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Gasolina()
        {
            return View();
        }
    }
}
