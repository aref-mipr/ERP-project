
namespace ERP.Application.Contract.ProductAgg
{
    public interface IApplicationProduct
    {
        void Create(CreateProductDto command);
        void Edit(EditProductDto command);
        ProductViewModel GetBy(int productId);
        List<ProductViewModel> GetAll();
        EditProductDto GetForEdit(int id);
        List<ProductViewModel> GetProductsByCategoryId(int id);
        void ChangeStockQuantity(int id, int quantity);
    }
}
