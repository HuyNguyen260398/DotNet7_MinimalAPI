using MagicVilla_Coupon_API.Models;

namespace MagicVilla_Coupon_API.Data;

public static class CouponStore
{
    public static List<Coupon> couponList = new()
    {
        new Coupon { Id = 1, Name="10OFF", Percent=10, IsActive=true },
        new Coupon { Id = 1, Name="20OFF", Percent=20, IsActive=false }
    };
}
