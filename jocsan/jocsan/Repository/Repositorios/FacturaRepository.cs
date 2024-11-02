using iTextSharp.text;
using iTextSharp.text.pdf;
using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace jocsan.Repository.Repositorios
{
    public class FacturaRepository : Repository<Factura>, IFacturaRepository
    {
        private readonly ApplicationDbContext _context;

        public FacturaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }     
        public async Task<int> ObtenerUltimoNumeroFacturaAsync()
        {
            // Obtener el máximo IdFactura y sumarle 1. Si no existen facturas, retornará 1.
            int nuevoIdFactura = (await _context.Factura.MaxAsync(f => (int?)f.IdFactura) ?? 0) + 1;
            return nuevoIdFactura;
        }
    }
}
