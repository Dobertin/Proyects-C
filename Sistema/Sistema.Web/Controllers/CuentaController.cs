using Microsoft.AspNetCore.Mvc;

namespace Sistema.Web.Controllers
{
    public class CuentaController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
                return RedirectToAction("Index", "Home");
           
        }
    }
}
