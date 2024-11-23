using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Database.Entities
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }
        public string? NombreRol { get; set; }
        public string? Descripcion { get; set; }
    }

}
