using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.DTOs.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreatePaymentIntentAsync(string basketId);
    }
}
