using Microsoft.AspNetCore.Mvc;

namespace Sistema.Web.Controllers
{
    public class TiendasController : Controller
    {
        public IActionResult Administracion()
        {
            return View();
        }

        public IActionResult Ventas()
        {
            return View();
        }
    }
}
