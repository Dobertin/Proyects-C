using jocsan.Data;
using jocsan.Models;
using jocsan.Models.results;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Repository.Repositorios
{
    public class GasolinaRepository : Repository<Gasolina>, IGasolinaRepository
    {
        private readonly ApplicationDbContext _context;

        public GasolinaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<GasolinaResult> ObtenerDatosGasolinaPorID(int idCliente)
        {
            // Obtener las sumas necesarias para los valores de total y cantidad
            var sumas = await _context.Gasolina
                .Where(g => g.IdCliente == idCliente)
                .GroupBy(g => g.IdCliente)
                .Select(g => new
                {
                    CantidadGalonPagado = g.Sum(x => x.CantGalonPagado),
                    TotalGalonPagado = g.Sum(x => x.TotalGalonPagado),
                    CantidadGalonCargado = g.Sum(x => x.CantGalonCargado),
                    TotalGalonCargado = g.Sum(x => x.TotalGalonCargado)
                })
                .FirstOrDefaultAsync();

            if (sumas == null)
            {
                return null; // Retornar null si no hay datos
            }

            // Calcular total de deuda y cantidad de deuda
            var totalGalonDeuda = sumas.TotalGalonPagado - sumas.TotalGalonCargado;
            var cantidadGalonDeuda = sumas.CantidadGalonPagado - sumas.CantidadGalonCargado;

            // Obtener los registros pagados para la lista pagados, excluyendo valores null o 0
            var pagados = await _context.Gasolina
                .Where(g => g.IdCliente == idCliente
                            && g.CantGalonPagado > 0
                            && g.PrecioGalonPagado > 0
                            && g.TotalGalonPagado > 0)
                .Select(g => new GasolinaResulttable
                {
                    precio = g.PrecioGalonPagado.ToString("F2"),
                    cantidad = g.CantGalonPagado.ToString(),
                    total = g.TotalGalonPagado.ToString("F2"),
                    fecha = g.FechaOperacion.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            // Obtener los registros cargados para la lista cargados, excluyendo valores null o 0
            var cargados = await _context.Gasolina
                .Where(g => g.IdCliente == idCliente
                            && g.CantGalonCargado > 0
                            && g.PrecioGalonCargado > 0
                            && g.TotalGalonCargado > 0)
                .Select(g => new GasolinaResulttable
                {
                    precio = g.PrecioGalonCargado.ToString("F2"),
                    cantidad = g.CantGalonCargado.ToString(),
                    total = g.TotalGalonCargado.ToString("F2"),
                    fecha = g.FechaOperacion.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            // Crear el objeto de resultado
            var gasolinaResult = new GasolinaResult
            {
                totalgalonpagado = sumas.TotalGalonPagado.ToString("F2"),
                cantidadgalonpagado = sumas.CantidadGalonPagado.ToString(),
                totalgaloncargado = sumas.TotalGalonCargado.ToString("F2"),
                cantidadgaloncargado = sumas.CantidadGalonCargado.ToString(),
                totalgalondeuda = totalGalonDeuda.ToString("F2"),
                cantidadgalondeuda = cantidadGalonDeuda.ToString(),
                pagados = pagados,
                cargados = cargados
            };

            return gasolinaResult;
        }

    }
}
