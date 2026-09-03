using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.DTOs.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder([FromBody] OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.CreateOrderAsync(orderDto, email!);
            return HandleResult(order);
        }
    }
}
