using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.Domain.Entities.OrderModule;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Domain.IdentityModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email)
        {
            var ordeAddress = _mapper.Map<ShippingAddress>(orderDto.shippingAddress);

            var basket = await _basketRepository.GetBasketAsync(orderDto.basketId);
            if (basket == null) return Error.NotFound("Basket not found", $"Basket with id {orderDto.basketId} is not found!");

            List<OrderItem> items = new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product == null) return Error.NotFound("Product not found", $"Product with id {item.Id} is not found!");

                var orderItem = CreateOrderItem(item, product);
                items.Add(orderItem);
            }

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.deliveryMethodId);
            if (deliveryMethod == null) return Error.NotFound("Delivery method not found", $"Delivery method with id {orderDto.deliveryMethodId} is not found!");

            var subTotal = items.Sum(i => i.Price * i.Quantity);
            var order = new Order()
            {
                UserEmail = email,
                Address = ordeAddress,
                DeliveryMethod = deliveryMethod,
                OrderItems = items,
                Subtotal = subTotal
            };

            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            int result = await _unitOfWork.SaveChangeAsync();
            if (result == 0) return Error.Failure("Order creation failed", "Failed to create order!");
            var readyOrder = _mapper.Map<OrderToReturnDto>(order);
            return readyOrder;

        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrder()
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureURL
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }
    }
}
