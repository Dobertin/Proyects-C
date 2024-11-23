using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sistema.Database.Context;
using Sistema.Database.Entities;
using Sistema.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Database.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MiContexto _context;
        private IDbContextTransaction _transaction;

        public IGenericRepository<Rol> Rol { get; }
        public IGenericRepository<Tienda> Tienda { get; }
        public IGenericRepository<Usuario> Usuario { get; }

        public UnitOfWork(MiContexto context)
        {
            _context = context;
            Rol = new GenericRepository<Rol>(context);
            Tienda = new GenericRepository<Tienda>(context);
            Usuario = new GenericRepository<Usuario>(context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

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
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
        }
    }


}
