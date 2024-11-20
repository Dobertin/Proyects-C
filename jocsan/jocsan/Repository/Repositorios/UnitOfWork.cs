using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace jocsan.Repository.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Cliente = new ClienteRepository(_context);
            Abono = new AbonoRepository(_context);
            Factura = new FacturaRepository(_context);
            Creditos = new CreditoRepository(_context);
            DetalleFactura = new DetalleFacturaRepository(_context);
            MovimientosCredito = new MovimientosCreditoRepository(_context);
            Producto = new ProductoRepository(_context);
            Gasolina = new GasolinaRepository(_context);
            Parametro = new ParametroRepository(_context);
            Vuelto = new VueltoRepository(_context);
        }

        public IClienteRepository Cliente { get; private set; }
        public IAbonoRepository Abono { get; private set; }
        public ICreditoRepository Creditos { get; private set; }
        public IDetalleFacturaRepository DetalleFactura { get; private set; }
        public IFacturaRepository Factura { get; private set; }
        public IMovimientosCreditoRepository MovimientosCredito { get; private set; }
        public IProductoRepository Producto { get; private set; }
        public IGasolinaRepository Gasolina { get; private set; }
        public IParametroRepository Parametro { get; private set; }
        public IVueltoRepository Vuelto { get; private set; }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
