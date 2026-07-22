namespace ERP.Application.Contract.ProductCategoryAgg
{
    public interface IApplicationProductCategory
    {
        void Create(CreateProductCategoryDto command);
        void Edit(EditProductCategoryDto command);
        List<ProductCategoryViewModel> GetAll();
        EditProductCategoryDto GetForEdit(int id);
        void Remove(int id);
        void Restore(int id);
    }
}
