using Microsoft.AspNetCore.Mvc;

namespace Sistema.Web.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Administracion()
        {
            return View();
        }
    }
}
