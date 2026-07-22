using ERP.Domain.Criteria;
using FluentValidation;

namespace ERP.Application.Contract.ProductAgg
{
    public class EditProductDto
    {
        public int Id { get; set; }
        public ProductCriteria ProductCriterias { get; set; }
    }
    public class EditProductValidator : AbstractValidator<EditProductDto>
    {
        public EditProductValidator()
        {
            RuleFor(x => x.ProductCriterias.Name)
                .NotEmpty()
                .MaximumLength(100).WithMessage("نام نباید بیش از 100 کاراکتر باشد .");

            RuleFor(x => x.ProductCriterias.CostPrice)
                .NotEmpty()
                .GreaterThan(0).WithMessage("قیمت باید بیشتر از 0 باشد");

            RuleFor(x => x.ProductCriterias.ProductCategoryId)
                .NotEmpty().WithMessage("دسته بندی را انتخاب کنید");
        }
    }
}
