using Microsoft.AspNetCore.Mvc;

namespace Sistema.Web.Models
{
    public class CuentaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
