using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Entity.DTO
{
    [DataContract]
    public class Ventas
    {
        [DataMember]
        public int idVenta { get; set; }
        [DataMember]
        public int idCliente { get; set; }
        [DataMember]
        public int idAsesor { get; set; }
        [DataMember]
        public int idProducto { get; set; }
        [DataMember]
        public string periodo { get; set; }
        [DataMember]
        public decimal puntosObtenidos { get; set; }
        [DataMember]
        public string fechaVenta { get; set; }
        [DataMember]
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
