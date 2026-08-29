using Ecommerce.Shared.DTOs.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.ProductServicesAbstraction
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string basketId);
        Task<BasketDto> CreateBasketAsync(BasketDto basket);
        Task<BasketDto> UpdateBasketAsync(BasketDto basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
