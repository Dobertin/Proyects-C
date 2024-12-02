using Microsoft.AspNetCore.Mvc;
using Sistema.Negocio.Querys;
using Sistema.Negocio.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistema.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly RolService _rolService;
        public CuentaController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var datocombo = _usuarioService.Authenticate(username, password);
            if (datocombo.IsCompleted)
            {

            }
            return RedirectToAction("Index", "Home");
           
        }
    }
}
