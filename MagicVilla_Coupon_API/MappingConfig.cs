using AutoMapper;
using MagicVilla_Coupon_API.Models;
using MagicVilla_Coupon_API.Models.DTO;

namespace MagicVilla_Coupon_API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
        CreateMap<Coupon, CouponDTO>().ReverseMap();
    }
}
