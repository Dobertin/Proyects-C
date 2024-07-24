using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class ClienteRepository
    {
        private readonly MongoDBContext _context;

        public ClienteRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.Find(_ => true).ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes.Find(c => c.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Cliente cliente)
        {
            cliente.ID = await GetNextSequenceValue("Clientes");
            await _context.Clientes.InsertOneAsync(cliente);
        }

        public async Task UpdateAsync(int id, Cliente cliente)
        {
            await _context.Clientes.ReplaceOneAsync(c => c.ID == id, cliente);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Clientes.DeleteOneAsync(c => c.ID == id);
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string nombre, string direccion, string telefono, string email)
        {
            var filterBuilder = Builders<Cliente>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(nombre))
            {
                filter &= filterBuilder.Eq(c => c.Nombre, nombre);
            }

            if (!string.IsNullOrEmpty(direccion))
            {
                filter &= filterBuilder.Eq(c => c.Direccion, direccion);
            }

            if (!string.IsNullOrEmpty(telefono))
            {
                filter &= filterBuilder.Eq(c => c.Telefono, telefono);
            }

            if (!string.IsNullOrEmpty(email))
            {
                filter &= filterBuilder.Eq(c => c.Email, email);
            }

            return await _context.Clientes.Find(filter).ToListAsync();
        }

        private async Task<int> GetNextSequenceValue(string collectionName)
        {
            var filter = Builders<Counter>.Filter.Eq(c => c.CollectionName, collectionName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var counter = await _context.Counters.FindOneAndUpdateAsync(filter, update, options);
            return counter.SequenceValue;
        }
    }
}
