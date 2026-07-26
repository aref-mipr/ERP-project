using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.ProductItemAgg
{
    public class CreateProductItemDto
    { 
        public ProductItemCriteria ProductItemCriterias { get; set; }
    }
    public class CreateProductValidator : AbstractValidator<CreateProductItemDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductItemCriterias.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("قیمت باید حداقل 0 باشد");
        }
    }
}