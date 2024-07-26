using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Facturacion.Models;

namespace Facturacion.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Cliente> Clientes => _database.GetCollection<Cliente>("Clientes");
        public IMongoCollection<Producto> Productos => _database.GetCollection<Producto>("Productos");
        public IMongoCollection<Counter> Counters => _database.GetCollection<Counter>("Counters");
    }
}
