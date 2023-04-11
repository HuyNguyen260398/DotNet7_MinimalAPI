using FluentValidation;
using MagicVilla_Coupon_API.Models.DTO;

namespace MagicVilla_Coupon_API.Models.Validations;

public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
{
    public CouponCreateValidation()
    {
        RuleFor(model => model.Name).NotEmpty();
        RuleFor(model => model.Percent).InclusiveBetween(1, 100);
    }
}
