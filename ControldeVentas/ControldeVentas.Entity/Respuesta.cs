using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Entity
{
    public class Respuesta
    {
        public bool resultado { get; set; }
        public string observacion { get; set; }
        public Respuesta()
        {
            this.resultado = false;
            this.observacion = string.Empty;
        }
    }
    public class VentasValidas
    {
        public string cliente { get; set; }
        public string nombreProducto { get; set; }
        public string asesor { get; set; }
        public string nomtoDesembolsado { get; set; }
        public string periodo { get; set; }
        public string puntosObtenidos { get; set; }
        public string fechaVenta { get; set; }
        public VentasValidas()
        {
            this.cliente = string.Empty;
            this.nombreProducto = string.Empty;
            this.asesor = string.Empty;
            this.nomtoDesembolsado = string.Empty;
            this.periodo = string.Empty;
            this.puntosObtenidos = string.Empty;
            this.fechaVenta = string.Empty;
        }
    }
}
