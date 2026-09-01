using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.Service.Exceptions;
using Ecommerce.ServiceAbstraction.ProductServicesAbstraction;
using Ecommerce.Shared.DTOs.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.ProductServices
{
    public class BasketService : IBasketService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;

        public BasketService(IMapper mapper, IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
        }
        public async Task<BasketDto> CreateBasketAsync(BasketDto basket)
        {
            var basketEntity = _mapper.Map<BasketDto, CustomerBasket>(basket);
            var result = await _basketRepository.CreateBasketAsync(basketEntity);
            if (result)
            {
                return basket;
            }
            else
            {
                throw new Exception("Failed to create basket.");
            }
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            var isDeleted = await _basketRepository.DeleteBasketAsync(basketId);
            return isDeleted;
        }

        public async Task<BasketDto?> GetBasketAsync(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            var basketEntity = await _basketRepository.GetBasketAsync(basketId);
            if (basketEntity == null)
            {
                throw new BasketNotFoundException(basketId);
            }
            return _mapper.Map<CustomerBasket, BasketDto>(basketEntity);
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            var basketEntity = _mapper.Map<BasketDto, CustomerBasket>(basket);
            var updatedBasketEntity = await _basketRepository.UpdateBasketAsync(basketEntity);
            return _mapper.Map<CustomerBasket, BasketDto>(updatedBasketEntity);
        }
    }
}
