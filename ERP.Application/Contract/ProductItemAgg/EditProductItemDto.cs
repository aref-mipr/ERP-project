using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.ProductItemAgg
{
    public class EditProductItemDto
    {
        public long Id { get; set; }
        public ProductItemCriteria ProductItemCriterias { get; set; }
    }
    public class EditProductValidator : AbstractValidator<EditProductItemDto>
    {
        public EditProductValidator()
        {
            RuleFor(x => x.ProductItemCriterias.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("قیمت باید حداقل 0 باشد");

            RuleFor(x => x.ProductItemCriterias.ProductItemStatus)
               .NotEmpty().WithMessage("یک وضعیت را انتخاب کنید");
        }
    }
}
