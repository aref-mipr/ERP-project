using ERP.Domain.Criteria;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ERP.Application.Contract.UserAgg
{
    public class RegisterUserDto
    {
        public UserCriteria UsersCriteria { get; set; }
        public string Password { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class RegisterUserValidator: AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UsersCriteria.FirstName).NotEmpty().WithMessage("نام نباید خالی باشد")
                .MinimumLength(3).MaximumLength(50).WithMessage("نام باید بین 3 تا 50 حرف باشد");

            RuleFor(x => x.UsersCriteria.LastName).NotEmpty().WithMessage("نام خانوادگی نباید خالی باشد")
                .MinimumLength(3).MaximumLength(50).WithMessage("نام خانوادگی باید بین 3 تا 50 حرف باشد");

            RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور نباید خالی باشد")
                .MinimumLength(8).WithMessage("رمز عبور باید بیشتر از 8 حرف باشد");

            RuleFor(x => x.UsersCriteria.PhoneNumber).NotEmpty().WithMessage("شماره تلفن نباید خالی باشد")
                .MinimumLength(11).MaximumLength(11).WithMessage("یک شماره تلفن معتبر وارد کنید");
        }
    }
}
