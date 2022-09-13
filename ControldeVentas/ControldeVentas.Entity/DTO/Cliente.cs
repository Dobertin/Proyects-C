using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Entity.DTO
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string tipoDoc { get; set; }
        public string nroDoc { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }

        public Cliente()
        {
            this.idCliente = 0;
            this.nombres = string.Empty;
            this.apellidos = string.Empty;
            this.tipoDoc = string.Empty;
            this.nroDoc = string.Empty;
            this.telefono = string.Empty;
            this.celular = string.Empty;
        }
    }
}
