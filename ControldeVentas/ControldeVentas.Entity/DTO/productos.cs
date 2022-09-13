using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Entity.DTO
{
    public class Productos
    {
        public int idProducto { get; set; }
        public string tipo { get; set; }
        public decimal puntos { get; set; }
        public Productos()
        {
            this.idProducto = 0;
            this.tipo = string.Empty;
            this.puntos = 0;
        }
    }
}
