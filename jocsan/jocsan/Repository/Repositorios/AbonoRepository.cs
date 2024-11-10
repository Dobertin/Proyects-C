using jocsan.Data;
using jocsan.Models;
using jocsan.Models.results;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Repository.Repositorios
{
    public class AbonoRepository : Repository<Abono>, IAbonoRepository
    {
        private readonly ApplicationDbContext _context;

        public AbonoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<AbonoResultExtended> ObtenerAbonoPorClienteTotalAsync(int idcliente)
        {
            var data = await _context.Abono
                .Where(c => c.IdCliente == idcliente) // Filtrar por idCliente
                .OrderByDescending(c => c.FechaAbono)
                .Select(c => new AbonoResult
                {
                    IdAbono = c.IdAbono,
                    Descripcion = c.Descripcion,
                    ValorAbono = c.ValorAbono,
                    FechaAbono = c.FechaAbono.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            var totalValorAbono = data.Sum(a => a.ValorAbono);

            var resultados = new AbonoResultExtended
            {
                Abonos = data,
                TotalValorAbono = totalValorAbono
            };

            return resultados;
        }
        public async Task<CuentaResult> ObtenerUltimoAbonoPorClienteAsync(int idcliente)
        {
            var data = await _context.Abono
                .Where(c => c.IdCliente == idcliente) // Filtrar por idCliente
                .OrderByDescending(c => c.FechaAbono)
                .Select(c => new CuentaResult
                {
                    IdCuenta = c.IdAbono,
                    Comentario = c.Descripcion,
                    Monto = c.ValorAbono,
                    FechaCuenta = c.FechaAbono.ToString("dd/MM/yyyy"),
                    NomCliente =c.Cliente.Nombre
                })
                .FirstOrDefaultAsync();
            return data;
        }
    }
}
