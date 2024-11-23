using Microsoft.AspNetCore.Mvc;
using Sistema.Negocio.Services;

namespace Sistema.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly RolService _rolService;
        public UsuariosController(UsuarioService usuarioService, RolService rolService)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
        }

        public IActionResult Administracion()
        {
            return View();
        }

        [HttpGet("/Rol/ObtenerCombo")]
        public async Task<IActionResult> ObtenerComboRolAsync()
        {
            var datocombo = await _rolService.ObtenerComboRolAsync();
            return Ok(datocombo);
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
