using jocsan.Data;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace jocsan.Controllers
{
    public class CreditoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreditoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Creditos()
        {
            return View();
        }

    }
}
