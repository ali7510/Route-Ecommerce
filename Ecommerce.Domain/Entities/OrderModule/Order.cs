using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ShippingAddress Address { get; set; } = default!;
        public int DeliveryMethodId { get; set; } // FK
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Subtotal { get; set; }
        public decimal Total => Subtotal + DeliveryMethod.Price;

        public string? PaymentIntentId { get; set; }
    }
}
