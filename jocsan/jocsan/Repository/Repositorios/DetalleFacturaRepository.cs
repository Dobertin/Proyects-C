using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;

namespace jocsan.Repository.Repositorios
{
    public class DetalleFacturaRepository : Repository<DetalleFactura>, IDetalleFacturaRepository
    {
        private readonly ApplicationDbContext _context;

        public DetalleFacturaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
