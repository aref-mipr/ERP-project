using ERP.Domain.Entity;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryProductItem
    {
        void Create(ProductItemModel product);
        ProductItemModel GetBy(long id);
        ProductItemModel GetBy(int productId);
        List<ProductItemModel> GetAll();
        List<ProductItemModel> GetIAlltemsInStock();
        List<ProductItemModel> GetAllReadyToSell();
        List<ProductItemModel> GetAllBy(int productId);
       // List<ProductItemModel> GetAllInOrder(int orderId);
        int CalculateCode(int productCode, int baseCode);
        bool IsExist(long id);
        void SaveChange();
    }
}
