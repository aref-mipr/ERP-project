using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.CustomerAgg
{
    public class CreateCustomerDto
    {
        public int Id { get; set; }
        public CustomerCriteria CustomerCriterias { get; set; }
    }
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.CustomerCriterias.FirstName)
                .NotEmpty().WithMessage("نام نمی تواند خالی باشد")
                .MaximumLength(50).WithMessage("نام نباید بیش از 50 کاراکتر باشد .");

            RuleFor(x => x.CustomerCriterias.LastName)
                .NotEmpty().WithMessage("نام خانوادگی نمی تواند خالی باشد")
                .MaximumLength(50).WithMessage("نام خانوادگی نباید بیش از 50 کاراکتر باشد .");

            RuleFor(x => x.CustomerCriterias.Phone)
                .NotEmpty().WithMessage("شماره همراه نمی تواند خالی باشد")
                .MaximumLength(11).MinimumLength(11).WithMessage("شماره همراه باید 11 رقم باشد .");

            RuleFor(x => x.CustomerCriterias.Email)
                .EmailAddress().WithMessage("لطفا ایمیل را با فرمت درست وارد کنید");
        }
    }
}
