using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.Persistence.NoSqlDbSettings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IMongoCollection<CustomerBasket> _basketsCollection;

        public BasketRepository(MongoContext context)
        {
            _basketsCollection = context.Database.GetCollection<CustomerBasket>("baskets");
        }


        public async Task<bool> CreateBasketAsync(CustomerBasket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            basket.ExpireAt = DateTime.UtcNow.AddDays(7); // Set expiration date to 7 days from now
            await _basketsCollection.InsertOneAsync(basket);
            return true;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            if(string.IsNullOrEmpty(basketId))
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            var result = await _basketsCollection.DeleteOneAsync(b => b.Id == basketId);
            return result.DeletedCount > 0;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            if(string.IsNullOrEmpty(basketId))
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            return await _basketsCollection.Find(b => b.Id == basketId).FirstOrDefaultAsync();
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            basket.ExpireAt = DateTime.UtcNow.AddDays(7); // Update expiration date to 7 days from now
            return await _basketsCollection.FindOneAndReplaceAsync(b => b.Id == basket.Id, basket, new FindOneAndReplaceOptions<CustomerBasket>
            {
                ReturnDocument = ReturnDocument.After
            });
        }
    }
}
