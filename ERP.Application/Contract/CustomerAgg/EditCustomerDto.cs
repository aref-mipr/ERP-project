using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.CustomerAgg
{
    public class EditCustomerDto
    {
        public int Id { get; set; }
        public CustomerCriteria CustomersCriteria { get; set; }
    }

    public class EditCustomerDtoValidator: AbstractValidator<EditCustomerDto>
    {
        public EditCustomerDtoValidator()
        {
            RuleFor(x => x.CustomersCriteria.FirstName)
    .NotEmpty().WithMessage("نام نمی تواند خالی باشد")
    .MaximumLength(50).WithMessage("نام نباید بیش از 50 کاراکتر باشد .");

            RuleFor(x => x.CustomersCriteria.LastName)
                .NotEmpty().WithMessage("نام نمی تواند خالی باشد")
                .MaximumLength(50).WithMessage("نام نباید بیش از 50 کاراکتر باشد .");

            RuleFor(x => x.CustomersCriteria.Phone)
                .NotEmpty().WithMessage("شماره همراه نمی تواند خالی باشد")
                .MaximumLength(11).MinimumLength(11).WithMessage("شماره همراه باید 11 رقم باشد .");

            RuleFor(x => x.CustomersCriteria.Email)
                .EmailAddress().WithMessage("لطفا ایمیل را با فرمت درست وارد کنید");
        }
    }
}
