using Microsoft.AspNetCore.Mvc;

namespace Sistema.Web.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Administracion()
        {
            return View();
        }
    }
}
