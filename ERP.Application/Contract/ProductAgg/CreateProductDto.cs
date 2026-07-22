using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Interface.Repository;
using FluentValidation;

namespace ERP.Application.Contract.ProductAgg
{
    public class CreateProductDto
    {
        public int Id { get; set; }
        public ProductCriteria ProductCriterias { get; set; }
        public ProductItemCriteria ProductItemCriterias { get; set; }
    }

    public class CreateProductValidator: AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductCriterias.Name)
                .NotEmpty().WithMessage("نام نمی تواند خالی باشد")
                .MaximumLength(100).WithMessage("نام نباید بیش از 100 کاراکتر باشد .");

            RuleFor(x => x.ProductCriterias.SellPrice)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("قیمت باید حداقل 0 باشد");

            RuleFor(x => x.ProductCriterias.CostPrice)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("قیمت باید حداقل 0 باشد");
                

            RuleFor(x => x.ProductCriterias.StockQuantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("موجودی انبار باید حداقل 0 باشد");

            RuleFor(x => x.ProductCriterias.ProductCategoryId)
                .NotEmpty().WithMessage("دسته بندی را انتخاب کنید");
        }
    }
}
