using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryProductCategory
    {
        void Create(ProductCategoryModel category);
        ProductCategoryModel GetBy(int id);
        List<ProductCategoryModel> GetAll();
        void SaveChange();
    }
}
