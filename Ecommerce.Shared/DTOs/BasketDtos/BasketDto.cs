using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs.BasketDtos
{
    // records are imutable that don't change and override getHashcode and tostring methods
    //records are the best choice in order to transfer data like DTOs
    public record BasketDto (string Id,
        ICollection<BasketItemDto> BasketItems,
        int? DeliveryMethodId,
        string? PaymentIntentId,
        string? ClientSecret,
        decimal? ShippingCost
        );
}
