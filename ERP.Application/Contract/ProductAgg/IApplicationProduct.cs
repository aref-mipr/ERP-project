
using ERP.Application.Contract.FilterAgg;

namespace ERP.Application.Contract.ProductAgg
{
    public interface IApplicationProduct
    {
        void Create(CreateProductDto command);
        void Edit(EditProductDto command);
        ProductViewModel GetBy(int productId);
        List<ProductViewModel> GetAll(FilterParamsDto? filterParams);
        EditProductDto GetForEdit(int id);
        List<ProductViewModel> GetProductsByCategoryId(int id);
        int GetCount(string? subject = null);
        void ChangeStockQuantity(int id, int quantity);
    }
}
