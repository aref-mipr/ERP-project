using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryProduct
    {
        void Create(ProductModel product);
        ProductModel GetBy(int id);
        List<ProductModel> GetAll();
        int CalculateCode(int categoryCode, int baseCode);
        bool IsExist(int id);
        void SaveChange();
    }
}
