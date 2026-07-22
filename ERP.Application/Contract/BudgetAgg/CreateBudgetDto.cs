using FluentValidation;

namespace ERP.Application.Contract.BudgetAgg
{
    public class CreateBudgetDto
    {
        public decimal Amount { get; set; }
    }

    public class CreateBudgetValidator: AbstractValidator<CreateBudgetDto>
    {
        public CreateBudgetValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("مقدار سرمایه را وارد کنید")
                .GreaterThan(0).WithMessage("مبلغ سرمایه باید بیشتر از 0 باشد");
        }
    }
}
