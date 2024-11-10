using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace jocsan.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        // Esta acci�n GET renderiza la vista del login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string? correo, string? contrase�a)
        {
            // L�gica para autenticar al usuario
            //if (true)
            if (correo == "usuario@jocsan.com" && contrase�a == "123456")
            {
                // Autenticaci�n exitosa
                return RedirectToAction("Index", "Home");
            }

            // Si falla, volver al login
            ModelState.AddModelError("", "Correo o contrase�a incorrectos.");
            return View();
        }
    }
}
