using AutoMapper;
using Ecommerce.Domain.Entities.OrderModule;
using Ecommerce.Domain.IdentityModule;
using Ecommerce.Shared.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddressDto, Address>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity)).ReverseMap();

            CreateMap<ShippingAddressDto,ShippingAddress>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.lastName))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country)).ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.BuyerEmail, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status)).ReverseMap();
        }
    }
}
