using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.OrderAgg
{
    public class EditOrderDto
    {
        public int Id { get; set; }
        public List<long> ProductItemIds { get; set; }
        public OrderCriteria OrdersCriteria { get; set; }
    }

    public class EditOrderValidator: AbstractValidator<EditOrderDto>
    {
        public EditOrderValidator()
        {
            RuleFor(x => x.OrdersCriteria.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("مبلغ تخفیف باید حداقل 0 باشد");
        }
    }
}
