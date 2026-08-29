using AutoMapper;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.Shared.DTOs.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();


            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}
