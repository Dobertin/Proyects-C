using Facturacion.Data;
using Facturacion.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class RolRepository
    {
        private readonly IMongoCollection<Rol> _roles;

        public RolRepository(MongoDBContext context)
        {
            _roles = context.Database.GetCollection<Rol>("Roles");
        }

        public async Task<List<Rol>> GetAllAsync()
        {
            return await _roles.Find(rol => true).ToListAsync();
        }

        public async Task<Rol> GetByIdAsync(int id)
        {
            return await _roles.Find<Rol>(rol => rol.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Rol rol)
        {
            await _roles.InsertOneAsync(rol);
        }

        public async Task UpdateAsync(int id, Rol rolIn)
        {
            await _roles.ReplaceOneAsync(rol => rol.ID == id, rolIn);
        }

        public async Task DeleteAsync(int id)
        {
            await _roles.DeleteOneAsync(rol => rol.ID == id);
        }
    }
}
