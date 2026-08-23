using ERP.Application.Contract.FilterAgg;

namespace ERP.Application.Contract.ProductCategoryAgg
{
    public interface IApplicationProductCategory
    {
        void Create(CreateProductCategoryDto command);
        void Edit(EditProductCategoryDto command);
        List<ProductCategoryViewModel> GetAll();
        List<ProductCategoryViewModel> GetAll(FilterParamsDto filterParams);
        EditProductCategoryDto GetForEdit(int id);
        int GetCount(string? subject = null);
        void Remove(int id);
        void Restore(int id);
    }
}
