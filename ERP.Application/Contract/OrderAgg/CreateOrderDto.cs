using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.OrderAgg
{
    public class CreateOrderDto
    {
        public int Id { get; set; }
        public List<long> ProductItemIds { get; set; }
        public OrderCriteria OrdersCriteria { get; set; }
    }

    public class CreateOrderValidator: AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {

            RuleFor(x => x.OrdersCriteria.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("مبلغ تخفیف باید حداقل 0 باشد");
        }
    }
}
