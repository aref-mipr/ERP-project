using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.SideExpenseAgg
{
    public class EditSideExpenseDto
    {
        public int Id { get; set; }
        public SideExpenseCriteria SideExpensesCriteria { get; set; }
    }

    public class EditSideExpenseValidator : AbstractValidator<EditSideExpenseDto>
    {
        public EditSideExpenseValidator()
        {
            RuleFor(x => x.SideExpensesCriteria.Title)
                .NotEmpty().WithMessage("یک عنوان وارد کنید")
                .MaximumLength(100).WithMessage("عنوان نباید بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.SideExpensesCriteria.Amount)
                .NotEmpty().WithMessage("مبلغ نباید خالی باشد")
                .GreaterThan(0).WithMessage("مبلغ باید بیشتر از 0 باشد");
        }
    }
}
