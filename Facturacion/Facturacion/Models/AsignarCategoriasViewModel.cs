using System.Collections.Generic;

namespace Facturacion.Models
{
    public class AsignarCategoriasViewModel
    {
        public int PromocionID { get; set; }
        public List<string> Categorias { get; set; } = new List<string>();
    }
}
