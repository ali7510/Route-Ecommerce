using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = null!;
        public ICollection<BasketItem> BasketItems { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpireAt { get; set; }

        #region Payment related Properties
        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal? ShippingCost { get; set; }
        #endregion
    }
}
