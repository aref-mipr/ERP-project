using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryProductItem
    {
        void Create(ProductItemModel product);
        ProductItemModel GetBy(long id);
        List<ProductItemModel> GetAll();
        int CalculateCode(int productCode, int baseCode);
        bool IsExist(long id);
        void SaveChange();
    }
}
