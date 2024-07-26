using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class TipoImpuestoRepository
    {
        private readonly MongoDBContext _context;

        public TipoImpuestoRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoImpuesto>> GetAllAsync()
        {
            return await _context.TiposImpuestos.Find(_ => true).ToListAsync();
        }

        public async Task<TipoImpuesto> GetByIdAsync(int id)
        {
            return await _context.TiposImpuestos.Find(ti => ti.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TipoImpuesto tipoImpuesto)
        {
            tipoImpuesto.ID = await GetNextSequenceValue("TiposImpuestos");
            await _context.TiposImpuestos.InsertOneAsync(tipoImpuesto);
        }

        public async Task UpdateAsync(int id, TipoImpuesto tipoImpuesto)
        {
            await _context.TiposImpuestos.ReplaceOneAsync(ti => ti.ID == id, tipoImpuesto);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.TiposImpuestos.DeleteOneAsync(ti => ti.ID == id);
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
