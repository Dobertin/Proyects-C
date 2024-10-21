using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;

namespace jocsan.Repository.Repositorios
{
    public class FacturaRepository : Repository<Factura>, IFacturaRepository
    {
        private readonly ApplicationDbContext _context;

        public FacturaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
