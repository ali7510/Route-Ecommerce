using Ecommerce.Domain.Entities.BasketModule;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Ecommerce.Persistence.NoSqlDbSettings
{
    // Infrastructure/Persistence/MongoIndexInitializer.cs
    public class MongoIndexInitializer
    {
        private readonly MongoContext _context;

        public MongoIndexInitializer(MongoContext context) => _context = context;

        public async Task InitializeAsync()
        {
            var collection = _context.Database.GetCollection<CustomerBasket>("Baskets");

            var indexKeys = Builders<CustomerBasket>.IndexKeys.Ascending(s => s.ExpireAt);
            var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };
            // ExpireAfter = TimeSpan.Zero means "expire at the value stored in ExpireAt"

            var indexModel = new CreateIndexModel<CustomerBasket>(indexKeys, indexOptions);
            await collection.Indexes.CreateOneAsync(indexModel);
        }
    }
}
