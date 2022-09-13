using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Entity
{
    public class Asesores
    {
        public int idAsesor { get; set; }
        public string usuario { get; set; }
        public string nombres { get; set; }
        public string apellido { get; set; }
        public string tipoDoc { get; set; }
        public string nroDoc { get; set; }
        public int cantVentas { get; set; }
        public int metaPropuesta { get; set; }
        public Asesores()
        {
            this.idAsesor = 0;
            this.usuario = string.Empty;
            this.nombres = string.Empty;
            this.apellido = string.Empty;
            this.tipoDoc = string.Empty;
            this.nroDoc = string.Empty;
            this.cantVentas = 0;
            this.metaPropuesta = 0;
        }
    }
}
