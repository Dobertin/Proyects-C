using jocsan.Data;
using jocsan.Models;
using jocsan.Models.querys;
using jocsan.Models.results;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace jocsan.Repository.Repositorios
{
    public class CreditoRepository : Repository<Credito>, ICreditoRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CreditoResults>> ObtenerCreditosPorClienteAsync(int idcliente)
        {
            var resultados = await _context.Creditos
                .Where(c => c.IdCliente == idcliente && c.Estado == 1) // Filtrar por idCliente y Estado
                .OrderByDescending(c => c.FechaCredito)
                .Select(c => new CreditoResults
                {
                    IdCredito = c.IdCredito,
                    Descripcion = c.Descripcion,
                    ValorCredito = c.ValorCredito,
                    FechaCredito = c.FechaCredito.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return resultados;
        }
        public async Task<CreditoResultsExtended> ObtenerCreditosPorClienteTotalAsync(int idcliente)
        {
            var data = await _context.Creditos
                .Where(c => c.IdCliente == idcliente && c.Estado == 1) // Filtrar por idCliente y Estado
                .OrderByDescending(c => c.FechaCredito)
                .Select(c => new CreditoResults
                {
                    IdCredito = c.IdCredito,
                    Descripcion = c.Descripcion,
                    ValorCredito = c.ValorCredito,
                    FechaCredito = c.FechaCredito.ToString("dd/MM/yyyy")
                })
            .ToListAsync();

            var totalValor = data.Sum(a => a.ValorCredito);

            var resultados = new CreditoResultsExtended
            {
                Creditos = data,
                TotalValorCreditos = totalValor
            };

            return resultados;

        }
        public async Task EliminarCreditoAsync(int idcredito)
        {
            // Obtener la factura por su Id
            var credito = await _context.Creditos.FindAsync(idcredito);

            if (credito == null)
            {
                throw new Exception("Factura no encontrada.");
            }

            // Modificar los valores de la entidad
            credito.Estado = 0;
            credito.FechaModifica = DateTime.Now;
            credito.UsuarioModifica = Environment.UserName; // Asigna el usuario actual del sistema

            // Marcar la entidad como modificada y guardar cambios
            _context.Creditos.Update(credito);
            await _context.SaveChangesAsync();
        }
    }
}
