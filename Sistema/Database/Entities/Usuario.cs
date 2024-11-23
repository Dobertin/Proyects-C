using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Database.Entities
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }

        [Column("usuario")]
        public string UsuarioNombre { get; set; }
        public string Contrasena { get; set; }
        public string? PathFoto { get; set; }
        public int IdRol { get; set; }
        public int? IdTienda { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public int UsuarioCreacion { get; set; }
        public int? UsuarioActualizacion { get; set; }

        [ForeignKey(nameof(IdRol))]
        public Rol Rol { get; set; }

        [ForeignKey(nameof(IdTienda))]
        public Tienda? Tienda { get; set; }
    }

}
