using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.AutoMapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //se mapea la entidad coupon a coupondto y viceversa
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }
    }
}
