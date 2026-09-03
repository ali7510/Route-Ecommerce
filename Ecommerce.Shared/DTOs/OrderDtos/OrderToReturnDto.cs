using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs.OrderDtos
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public ShippingAddressDto ShippingAddress { get; set; } = default!;
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string DeliveryMethod { get; set; } = default!;
        public string OrderStatus { get; set; } = default!;
    }
}
