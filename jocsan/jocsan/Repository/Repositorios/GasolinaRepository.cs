using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;

namespace jocsan.Repository.Repositorios
{
    public class GasolinaRepository : Repository<Gasolina>, IGasolinaRepository
    {
        private readonly ApplicationDbContext _context;

        public GasolinaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
