using Ecommerce.ServiceAbstraction.ProductServicesAbstraction;
using Ecommerce.Shared.DTOs.BasketDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string basketId)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateBasket(BasketDto basket)
        {
            var createdBasket = await _basketService.CreateBasketAsync(basket);
            return Ok(createdBasket);
        }

        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var isDeleted = await _basketService.DeleteBasketAsync(basketId);
            return Ok(isDeleted);
        }
    }
}