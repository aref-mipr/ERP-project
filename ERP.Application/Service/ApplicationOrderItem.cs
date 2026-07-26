using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.OrderItemAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Interface.Repository;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Application.Service
{
    public class ApplicationOrderItem : IApplicationOrderItem
    {
        private readonly IRepositoryOrderItem _repositoryOrderItem;
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationBudget _applicationBudget;
        public ApplicationOrderItem(IRepositoryOrderItem repositoryOrderItem, IRepositoryProductItem repositoryProductItem,
            IApplicationProductItem applicationProductItem, IApplicationFinancialTransaction applicationFinancialTransaction,
            IApplicationBudget applicationBudget)
        {
            _repositoryOrderItem = repositoryOrderItem;
            _repositoryProductItem = repositoryProductItem;
            _applicationProductItem = applicationProductItem;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationBudget = applicationBudget;
        }

        public List<OrderItemViewModel> GetAllBy(int orderId)
        {
            return _repositoryOrderItem.GetAllBy(orderId).Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                ProductItemId = x.ProductItem.Id,
                ProductItemCode = x.ProductItem.ProductItemCode,
                ProductName = x.ProductItem.Product.Name,
                Price = x.ProductItem.Price,
                Returned = x.Returned,
            }).OrderBy(x => x.ProductItemCode).ToList();
        }

        public void Return(long id)
        {
            var quary = _repositoryOrderItem.GetBy(id);
            if (quary == null)
                throw new NullReferenceException();

            var productItem = _repositoryProductItem.GetAll().FirstOrDefault(x => x.Id == quary.ProductItemId);
            if (productItem == null)
                throw new NullReferenceException();

            quary.Return();
            _applicationProductItem.ChangeStatus(productItem.Id, ProductItemStatuses.Testing);

            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    OrderItemId = id,
                    TransactionType = TransactionTypes.ReturnedOrderItem,
                    Amount = -quary.Price,
                }
            };
            _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
            _applicationFinancialTransaction.Create(commandTransaction);
            _repositoryOrderItem.SaveChange();
        }
    }
}
