using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class ProductoRepository
    {
        private readonly MongoDBContext _context;

        public ProductoRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos.Find(_ => true).ToListAsync();
        }

        public async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Productos.Find(p => p.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Producto producto)
        {
            producto.ID = await GetNextSequenceValue("Productos");
            await _context.Productos.InsertOneAsync(producto);
        }

        public async Task UpdateAsync(int id, Producto producto)
        {
            await _context.Productos.ReplaceOneAsync(p => p.ID == id, producto);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Productos.DeleteOneAsync(p => p.ID == id);
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
