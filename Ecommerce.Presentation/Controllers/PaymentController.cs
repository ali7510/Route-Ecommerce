using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.DTOs.BasketDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost("create-payment-intent/{basketId}")]
        [Authorize]
        public async Task<ActionResult<BasketDto>> CreatePaymentIntent(string basketId)
        {
            var result = await _paymentService.CreatePaymentIntentAsync(basketId);
            return HandleResult(result);
        }
    }
}
