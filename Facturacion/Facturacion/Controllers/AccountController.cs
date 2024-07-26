using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Facturacion.Models;
using Facturacion.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using BCrypt.Net;

namespace Facturacion.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;

        public AccountController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
                await _usuarioRepository.AddAsync(usuario);
                return RedirectToAction("Login");
            }
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string nombreUsuario, string contraseña)
        {
            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.Email)
                };

                foreach (var rol in usuario.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
