using ERP.Application.Contract.FilterAgg;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Application.Contract.ProductItemAgg
{
    public interface IApplicationProductItem
    {
        void Create(CreateProductItemDto command);
        void Edit(EditProductItemDto command);
        ProductItemViewModel GetBy(long id);
        CreateProductItemDto GetBy(int productId);
        List<ProductItemViewModel> GetAll();
        List<ProductItemViewModel> GetAllReadyToSell(int id);
        List<ProductItemViewModel> GetAllBy(int productId);
        EditProductItemDto GetForEdit(long id);
        List<ProductItemViewModel> GetIAlltemsInWarehouse(FilterParamsDto filterParams);
        int GetCount(string? subject = null);
        int GetCountInWarehouse(string? subject = null);
        void ChangeStatus(long id, ProductItemStatuses status);
        List<ProductItemStatusViewModel> CreateStatuses();
    }
}
