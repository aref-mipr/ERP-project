using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.EmployeeAgg
{
    public class CreateEmployeeDto
    {
        public EmployeeCriteria EmployeesCriteria { get; set; }
    }

    public class CreateEmployeeValidator: AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.EmployeesCriteria.FirstName)
                .NotEmpty().WithMessage("نام نمی تواند خالی باشد")
                .MaximumLength(50).WithMessage("نام نباید بیش از 50 کاراکتر باشد .");

            RuleFor(x => x.EmployeesCriteria.LastName)
                .NotEmpty().WithMessage("نام خانوادگی نمی تواند خالی باشد")
                .MaximumLength(50).WithMessage("نام خانوادگی نباید بیش از 50 کاراکتر باشد .");

            RuleFor(x => x.EmployeesCriteria.Phone)
                .NotEmpty().WithMessage("شماره همراه نمی تواند خالی باشد")
                .MaximumLength(11).MinimumLength(11).WithMessage("شماره همراه باید 11 رقم باشد .");

            RuleFor(x => x.EmployeesCriteria.Position)
                .NotEmpty().WithMessage("این فیلد نباید خالی باشد");

            RuleFor(x => x.EmployeesCriteria.SalaryMonthly)
                .NotEmpty().WithMessage("این فیلد نباید خالی باشد")
                .GreaterThanOrEqualTo(0).WithMessage(" دستمزد باید حداقل 0 باشد");
        }
    }
}
