using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email);
    }
}
