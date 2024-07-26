using MongoDB.Driver;
using Facturacion.Models;
using Facturacion.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class ListaPrecioRepository
    {
        private readonly MongoDBContext _context;

        public ListaPrecioRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListaPrecio>> GetAllAsync()
        {
            return await _context.ListasPrecios.Find(_ => true).ToListAsync();
        }

        public async Task<ListaPrecio> GetByIdAsync(int id)
        {
            return await _context.ListasPrecios.Find(lp => lp.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(ListaPrecio listaPrecio)
        {
            listaPrecio.ID = await GetNextSequenceValue("ListasPrecios");
            await _context.ListasPrecios.InsertOneAsync(listaPrecio);
        }

        public async Task UpdateAsync(int id, ListaPrecio listaPrecio)
        {
            await _context.ListasPrecios.ReplaceOneAsync(lp => lp.ID == id, listaPrecio);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.ListasPrecios.DeleteOneAsync(lp => lp.ID == id);
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
