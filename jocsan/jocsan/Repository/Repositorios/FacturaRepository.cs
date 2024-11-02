using iTextSharp.text;
using iTextSharp.text.pdf;
using jocsan.Data;
using jocsan.Models;
using jocsan.Models.querys;
using jocsan.Models.results;
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
        public async Task<IEnumerable<FacturaResults>> ObtenerFacturasAsync(FacturaQuery facturaQuery)
        {
            var query = _context.Factura.AsQueryable();

            query = query.Where(f => f.Estado == 1);

            // Aplicar filtros solo si los valores están definidos (no nulos o no cero)
            if (facturaQuery.IdFactura > 0)
            {
                query = query.Where(f => f.IdFactura == facturaQuery.IdFactura);
            }

            if (facturaQuery.IdCliente > 0)
            {
                query = query.Where(f => f.IdCliente == facturaQuery.IdCliente);
            }

            var resultados = await query
                .Select(f => new FacturaResults
                {
                    IdFactura = f.IdFactura,
                    Numproductos = _context.DetalleFactura
                                            .Where(df => df.IdFactura == f.IdFactura)
                                            .Count() + " Productos",
                    Total = f.TotalVenta,
                    Fecha = f.FechaVenta.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return resultados;
        }
        public async Task EliminarFacturaAsync(int idFactura)
        {
            // Obtener la factura por su Id
            var factura = await _context.Factura.FindAsync(idFactura);

            if (factura == null)
            {
                throw new Exception("Factura no encontrada.");
            }

            // Modificar los valores de la entidad
            factura.Estado = 0;
            factura.FechaModifica = DateTime.Now;
            factura.UsuarioModifica = Environment.UserName; // Asigna el usuario actual del sistema

            // Marcar la entidad como modificada y guardar cambios
            _context.Factura.Update(factura);
            await _context.SaveChangesAsync();
        }
    }
}
