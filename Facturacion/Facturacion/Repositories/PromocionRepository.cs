using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class PromocionRepository
    {
        private readonly MongoDBContext _context;

        public PromocionRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promocion>> GetAllAsync()
        {
            return await _context.Promociones.Find(_ => true).ToListAsync();
        }

        public async Task<Promocion> GetByIdAsync(int id)
        {
            return await _context.Promociones.Find(p => p.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Promocion promocion)
        {
            promocion.ID = await GetNextSequenceValue("Promociones");
            await _context.Promociones.InsertOneAsync(promocion);
        }

        public async Task UpdateAsync(int id, Promocion promocion)
        {
            await _context.Promociones.ReplaceOneAsync(p => p.ID == id, promocion);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Promociones.DeleteOneAsync(p => p.ID == id);
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
