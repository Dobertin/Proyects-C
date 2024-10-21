using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;

namespace jocsan.Repository.Repositorios
{
    public class AbonoRepository : Repository<Abono>, IAbonoRepository
    {
        private readonly ApplicationDbContext _context;

        public AbonoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
