using ControldeVentas.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Bussiness
{
    public class BOVenta
    {
        public void actualizarMetaAsesor(int idAsesor, int cantidad)
        {
            DOVenta doVenta = new DOVenta();
            doVenta.actualizarMetaAsesor(idAsesor, cantidad);
        }
    }
}
