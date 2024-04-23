using AutoMapper;
using Microsoft.Extensions.Options;
using Store.Data.Entities.identityEntites;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService.Dtos
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<Address , AddressDto>().ReverseMap();
            CreateMap<AddressDto,ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.DeliveryMethodName, option => option.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, option => option.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductItemId, options => options.MapFrom(src => src.ItemOrder.ProductItemId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.ItemOrder.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.ItemOrder.PictureUrl))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemUrlResolver>());
        }
    }
}
