using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.ProductCategoryAgg
{
    public class EditProductCategoryDto
    {
        public int Id { get; set; }
        public ProductCategoryCriteria ProductCategoryCriterias { get; set; }
    }
    public class EditProductCategoryDtoValidator : AbstractValidator<EditProductCategoryDto>
    {
        public EditProductCategoryDtoValidator()
        {
            RuleFor(x => x.ProductCategoryCriterias.Name)
                .NotEmpty().MaximumLength(100).WithMessage("نام نباید بیش از 100 کاراکتر باشد .");
        }
    }
}
