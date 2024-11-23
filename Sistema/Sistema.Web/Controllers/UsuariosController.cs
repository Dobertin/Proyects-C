using Microsoft.AspNetCore.Mvc;
using Sistema.Negocio.Services;

namespace Sistema.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;
        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Administracion()
        {
            return View();
        }

        [HttpGet("/Usuarios/Obtener")]
        public async Task<IActionResult> GetUsuariosAsync()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpPost("cambiar-tienda")]
        public async Task<IActionResult> CambiarTiendaAsync(int idUsuario, int idTienda, int idAdmin)
        {
            try
            {
                await _usuarioService.CambiarTiendaAsync(idUsuario, idTienda, idAdmin);
                return Ok("Tienda cambiada con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
