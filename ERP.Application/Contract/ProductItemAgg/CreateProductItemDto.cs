using ERP.Domain.Criteria;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Application.Contract.ProductItemAgg
{
    public class CreateProductItemDto
    { 
        public long Id { get; set; }
        public ProductItemCriteria ProductItemCriterias { get; set; }
    }
    public class CreateProductValidator : AbstractValidator<CreateProductItemDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductItemCriterias.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("قیمت باید حداقل 0 باشد");

            RuleFor(x => x.ProductItemCriterias.ProductItemStatus)
                .NotEmpty().WithMessage("یک وضعیت را انتخاب کنید");
        }
    }
}