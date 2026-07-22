using static ERP.Domain.Entity.EmployeeModel;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.OrderModel;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Domain.Interface.Utility
{
    public interface IEnumExtension
    {
        string OrderStatusesToPersianString(OrderStatuses status);
        string ItemStatusesToPersianString(ProductItemStatuses status);
        string EmployeeStatusesToPersianString(EmployeeStatuses status);
        string TransactionTypesToPersianString(TransactionTypes status);
    }
}
