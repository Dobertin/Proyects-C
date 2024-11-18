using Microsoft.AspNetCore.Mvc;

namespace Sistema.Web.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Inventarios()
        {
            return View();
        }

        public IActionResult Listado()
        {
            return View();
        }
    }
}
