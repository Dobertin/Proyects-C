using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Entity.DTO
{
    public class Ventas
    {
        public int idVenta { get; set; }
        public int idCliente { get; set; }
        public int idAsesor { get; set; }
        public int idProducto { get; set; }
        public string periodo { get; set; }
        public decimal puntosObtenidos { get; set; }
        public string fechaVenta { get; set; }
        public decimal montoDesembolsado { get; set; }
        public Ventas()
        {
            this.idVenta = 0;
            this.idCliente = 0;
            this.idAsesor = 0;
            this.idProducto = 0;
            this.periodo = string.Empty;
            this.puntosObtenidos = 0;
            this.fechaVenta = string.Empty;
            this.montoDesembolsado = 0;
        }
    }
}
