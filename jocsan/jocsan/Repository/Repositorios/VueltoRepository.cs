using jocsan.Data;
using jocsan.Models;
using jocsan.Models.results;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Repository.Repositorios
{
    public class VueltoRepository : Repository<Vuelto>, IVueltoRepository
    {
        private readonly ApplicationDbContext _context;

        public VueltoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<VueltoResultExtended> ObtenerVueltoPorClienteTotalAsync(int idcliente)
        {
            try
            {
                // Obtener datos de tipo "CARGO"
                var data = await _context.Vuelto
                    .Where(c => c.IdCliente == idcliente && c.TipoVuelto == 0) // Filtrar por idCliente y tipoVuelto "CARGO"
                    .OrderByDescending(c => c.FechaVuelto)
                    .Select(c => new VueltoResultData
                    {
                        IdVuelto = c.IdVuelto,
                        Descripcion = string.IsNullOrEmpty(c.Comentario) ? "Sin descripción" : c.Comentario,
                        Valor = c.Monto,
                        Fecha = c.FechaVuelto.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                var totalValorCargo = data?.Sum(a => a.Valor) ?? 0;

                // Obtener datos de tipo "ABONO"
                var dataAbono = await _context.Vuelto
                    .Where(c => c.IdCliente == idcliente && c.TipoVuelto == 1) // Filtrar por idCliente y tipoVuelto "ABONO"
                    .OrderByDescending(c => c.FechaVuelto)
                    .Select(c => new VueltoResultData
                    {
                        IdVuelto = c.IdVuelto,
                        Descripcion = string.IsNullOrEmpty(c.Comentario) ? "Sin descripción" : c.Comentario,
                        Valor = c.Monto,
                        Fecha = c.FechaVuelto.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                var totalValorAbono = dataAbono?.Sum(a => a.Valor) ?? 0;

                // Manejar casos nulos con listas vacías para evitar excepciones
                data = data ?? new List<VueltoResultData>();
                dataAbono = dataAbono ?? new List<VueltoResultData>();

                // Crear objetos de resultados
                var resultadosCargo = new VueltoResult
                {
                    Vuelto = data,
                    TotalVuelto = totalValorCargo
                };

                var resultadosAbono = new VueltoResult
                {
                    Vuelto = dataAbono,
                    TotalVuelto = totalValorAbono
                };

                var resultado = new VueltoResultExtended
                {
                    VueltoCargo = resultadosCargo,
                    VueltoAbono = resultadosAbono,
                    TotalVueltoGeneral = totalValorCargo - totalValorAbono
                };

                return resultado;
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                throw new Exception($"Error al obtener el vuelto para el cliente con ID {idcliente}: {ex.Message}", ex);
            }
        }
    }
}
