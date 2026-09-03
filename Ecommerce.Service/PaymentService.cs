using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.Domain.Entities.OrderModule;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.DTOs.BasketDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto>> CreatePaymentIntentAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return Error.NotFound("Basket not found", $"Basket with id {basketId} not found");

            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Domain.Entities.ProductModule.Product, int>().GetByIdAsync(item.Id);
                if (product is null) return Error.NotFound("Product not found", $"Product with id {item.Id} not found");
                item.Price = product.Price;
            }
            var subTotla = basket.BasketItems.Sum(x => x.Price * x.Quantity);
            if (!basket.DeliveryMethodId.HasValue)
            {
                return Error.NotFound("Delivery method not selected", "Please select a delivery method before proceeding to payment");
            }
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            basket.ShippingCost = deliveryMethod?.Price??0;
            var amount = subTotla + basket.ShippingCost;

            StripeConfiguration.ApiKey = _configuration["StripeOption:SecretKey"];
            PaymentIntentService paymentIntentService = new PaymentIntentService(); // ready class in Stripe.net package

            PaymentIntent paymentIntent; // in Stripe.net package
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Stripe expects amount in cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(amount * 100) // Stripe expects amount in cents
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            basket = await _basketRepository.UpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
