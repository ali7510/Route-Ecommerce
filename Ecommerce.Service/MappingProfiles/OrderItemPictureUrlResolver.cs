using AutoMapper;
using AutoMapper.Execution;
using Ecommerce.Domain.Entities.OrderModule;
using Ecommerce.Shared.DTOs.OrderDtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return string.Empty;
            }

            if (source.Product.PictureUrl.StartsWith("http"))
            {
                return source.Product.PictureUrl;
            }

            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                return string.Empty;
            }

            return $"{baseUrl}/{source.Product.PictureUrl}";
        }
    }
}
