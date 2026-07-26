using FluentValidation;
using ERP.Domain.Criteria;

namespace ERP.Application.Contract.ProductCategoryAgg
{
    public class CreateProductCategoryDto
    {
        public ProductCategoryCriteria ProductCategoryCriterias { get; set; }
        
    }
    public class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryDto>
    {
        public CreateProductCategoryValidator()
        {
            RuleFor(x => x.ProductCategoryCriterias.Name)
                .NotEmpty().MaximumLength(100).WithMessage("نام نباید بیش از 100 کاراکتر باشد .");
        }
    }

}