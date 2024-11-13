using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Models;

namespace ECommerce.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderCreateDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemCreateDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemUpdateDTO>().ReverseMap();

            CreateMap<Shipping, ShippingDTO>().ReverseMap();
            CreateMap<Shipping, ShippingCreateDTO>().ReverseMap();
            CreateMap<Shipping, ShippingUpdateDTO>().ReverseMap();

            CreateMap<CartItems, CartItemDTO>().ReverseMap();
            CreateMap<CartItems, CartItemCreateDTO>().ReverseMap();
            CreateMap<CartItems, CartItemUpdateDTO>().ReverseMap();

            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, ReviewCreateDTO>().ReverseMap();
            CreateMap<Review, ReviewUpdateDTO>().ReverseMap();
        }
    }
}
