using ERP.Domain.Interface.Utility;
using static ERP.Domain.Entity.EmployeeModel;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.OrderModel;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Infrastructure.Utility
{
    public class EnumExtension: IEnumExtension
    {
        public string OrderStatusesToPersianString(OrderStatuses status)
        {
            return status switch
            {
                OrderStatuses.Pending => "در انتظار تایید",
                OrderStatuses.Approved => "تایید شده",
                OrderStatuses.Canceled => "رد شده",
                _ => status.ToString()
            };
        }

        public string ItemStatusesToPersianString(ProductItemStatuses status)
        {
            return status switch
            {
                ProductItemStatuses.Testing => "در حال تست",
                ProductItemStatuses.Approved => "تایید شده",
                ProductItemStatuses.Returned => "مرجوعی",
                ProductItemStatuses.Selled => "فروخته شده",
                ProductItemStatuses.Unsellable => "غیر قابل فروش",
                ProductItemStatuses.ThrownOut => "دور ریخته شده",
                ProductItemStatuses.WaitingOrder => "در انتظار تایید سفارش",
                _ => status.ToString()
            };
        }

        public string EmployeeStatusesToPersianString(EmployeeStatuses status)
        {
            return status switch
            {
                EmployeeStatuses.Active => "مشغول به کار",
                EmployeeStatuses.Resigned => "استعغا داده",
                EmployeeStatuses.Suspended => "تعلیق شده",
                EmployeeStatuses.Fired => "اخراج شده",
                EmployeeStatuses.ReEmployment => "استخدام مجدد",
                _ => status.ToString()
            };
        }

        public string TransactionTypesToPersianString(TransactionTypes status)
        {
            return status switch
            {
                TransactionTypes.OpeningBalance => "سرمایه اولیه",
                TransactionTypes.Purchase => "خرید کالا",
                TransactionTypes.ReturnedProduct => "مرجوع شده توسط فروشگاه",
                TransactionTypes.Sale => "ثبت سفارش",
                TransactionTypes.ReturnedOrderItem => "مرجوع شده توسط مشتری",
                TransactionTypes.Salary => "پرداخت دستمزد ",
                TransactionTypes.Expence => "هزینه جانبی",
                TransactionTypes.Adjustment => "اصلاحیه",
                TransactionTypes.IncreaseBudget => "افزایش سرمایه",
                TransactionTypes.OnerWithdrawal => "برداشت شخصی از سرمایه",
                _ => status.ToString()
            };
        }
    }
}
