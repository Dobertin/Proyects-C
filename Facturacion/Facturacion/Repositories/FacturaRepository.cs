using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class FacturaRepository
    {
        private readonly MongoDBContext _context;

        public FacturaRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _context.Facturas.Find(_ => true).ToListAsync();
        }

        public async Task<Factura> GetByIdAsync(int id)
        {
            return await _context.Facturas.Find(f => f.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Factura factura)
        {
            factura.ID = await GetNextSequenceValue("Facturas");
            await _context.Facturas.InsertOneAsync(factura);
        }

        public async Task UpdateAsync(int id, Factura factura)
        {
            await _context.Facturas.ReplaceOneAsync(f => f.ID == id, factura);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Facturas.DeleteOneAsync(f => f.ID == id);
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
