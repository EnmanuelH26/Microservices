using AutoMapper;
using Fruit.Service.ProductAPI.Models;
using Fruit.Service.ProductAPI.Models.Dto;

namespace Fruit.Service.ProductAPI.AutoMapper
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDto>().ReverseMap(); //se mapea la entidad product a productdto y viceversa
        }
    }
}
