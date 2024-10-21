using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;

namespace jocsan.Repository.Repositorios
{
    public class CreditoRepository : Repository<Credito>, ICreditoRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
