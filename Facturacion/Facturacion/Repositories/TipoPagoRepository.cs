using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class TipoPagoRepository
    {
        private readonly MongoDBContext _context;

        public TipoPagoRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoPago>> GetAllAsync()
        {
            return await _context.TiposPagos.Find(_ => true).ToListAsync();
        }

        public async Task<TipoPago> GetByIdAsync(int id)
        {
            return await _context.TiposPagos.Find(tp => tp.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TipoPago tipoPago)
        {
            tipoPago.ID = await GetNextSequenceValue("TiposPagos");
            await _context.TiposPagos.InsertOneAsync(tipoPago);
        }

        public async Task UpdateAsync(int id, TipoPago tipoPago)
        {
            await _context.TiposPagos.ReplaceOneAsync(tp => tp.ID == id, tipoPago);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.TiposPagos.DeleteOneAsync(tp => tp.ID == id);
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
