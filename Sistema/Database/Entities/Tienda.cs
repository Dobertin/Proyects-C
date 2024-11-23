using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Database.Entities
{
    public class Tienda
    {
        [Key]
        public int IdTienda { get; set; }
        public string NombreTienda { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
    }

}
