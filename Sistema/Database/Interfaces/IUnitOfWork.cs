using Sistema.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Database.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Rol> Rol { get; }
        IGenericRepository<Tienda> Tienda { get; }
        IGenericRepository<Usuario> Usuario { get; }
        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

}
