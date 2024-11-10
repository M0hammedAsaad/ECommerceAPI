using AutoMapper;
using ECommerce.Core.Dtos;
using ECommerce.Core.Models;

namespace ECommerce.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
