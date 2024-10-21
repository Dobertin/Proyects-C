using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;

namespace jocsan.Repository.Repositorios
{
    public class MovimientosCreditoRepository : Repository<MovimientosCredito>, IMovimientosCreditoRepository
    {
        private readonly ApplicationDbContext _context;

        public MovimientosCreditoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
