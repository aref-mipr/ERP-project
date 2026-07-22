using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.OrderModel;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Application.Service
{
    public class ApplicationOrder : IApplicationOrder
    {
        private readonly IRepositoryOrder _repositoryOrder;
        private readonly IRepositoryOrderItem _repositoryOrderItem;
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IRepositoryCustomer _repositoryCustomer;
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IEnumExtension _enumExtension;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationBudget _applicationBudget;
        public ApplicationOrder(IRepositoryOrder repositoryOrder, IRepositoryOrderItem repositoryOrderItem,
            IRepositoryCustomer repositoryCustomer, IRepositoryProductItem repositoryProductItem,
            IApplicationProductItem applicationProductItem, IEnumExtension enumExtension,
            IApplicationFinancialTransaction applicationFinancialTransaction, IApplicationBudget applicationBudget)
        {
            _repositoryOrder = repositoryOrder;
            _repositoryOrderItem = repositoryOrderItem;
            _repositoryProductItem = repositoryProductItem;
            _repositoryCustomer = repositoryCustomer;
            _applicationProductItem = applicationProductItem;
            _enumExtension = enumExtension;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationBudget = applicationBudget;
        }

        public void Create(CreateOrderDto command)
        {
            if (_repositoryOrder.IsExist(command.Id))
                throw new NullReferenceException();

            int baseOrderCode = _repositoryOrder.GetAll().Count();

            var items = _repositoryProductItem.GetAllReadyToSell()
               .Where(x => command.ProductItemIds.Contains(x.Id)).ToList();

            var orderCriteria = new OrderCriteria();

            orderCriteria.CustomerId = command.OrdersCriteria.CustomerId;
            orderCriteria.OrderCode = baseOrderCode + 1;
            orderCriteria.Description = command.OrdersCriteria.Description;
            orderCriteria.InitialAmount = command.OrdersCriteria.InitialAmount;
            orderCriteria.DiscountAmount = command.OrdersCriteria.DiscountAmount;
            orderCriteria.FinalAmount = command.OrdersCriteria.FinalAmount;

            var order = new OrderModel(orderCriteria);
            _repositoryOrder.Create(order);
            _repositoryOrder.SaveChange();

            foreach (var item in items)
            {
                var itemCriteria = new OrderItemCriteria();
                itemCriteria.OrderId = order.Id;
                itemCriteria.ProductItemId = item.Id;
                itemCriteria.Price = item.Price;
                _applicationProductItem.ChangeStatus(item.Id, ProductItemStatuses.WaitingOrder);
                var orderItem = new OrderItemModel(itemCriteria);
                _repositoryOrderItem.Create(orderItem);
            }
            _repositoryOrderItem.SaveChange();
        }

        public List<OrderViewModel> GetAll()
        {
            return _repositoryOrder.GetAll().Select(x => new OrderViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                CustomerFullName = $"{x.Customer.FirstName} {x.Customer.LastName}",
                OrderStatus = _enumExtension.OrderStatusesToPersianString(x.OrderStatus),
                OrdersCriteria = new OrderCriteria
                {
                    OrderCode = x.OrderCode,
                }
            }).OrderBy(x => x.OrdersCriteria.OrderCode).ToList();
        }

        public OrderViewModel GetBy(int id)
        {
            var order = _repositoryOrder.GetBy(id);
            return new OrderViewModel
            {
                Id = order.Id,
                Description = order.Description,
                InitialAmount = order.InitialAmount,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                CustomerFullName = $"{order.Customer.FirstName} {order.Customer.LastName}",
                CustomerCode = order.Customer.SubscriptionCode,
                CreationTime = order.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                OrderStatus = _enumExtension.OrderStatusesToPersianString(order.OrderStatus),
                OrdersCriteria = new OrderCriteria
                {
                    OrderCode = order.OrderCode,
                }
            };
        }
        public List<OrderStatusViewModel> CreateStatuses()
        {
            var statuses = new List<OrderStatusViewModel>();

            foreach (OrderStatuses status in Enum.GetValues(typeof(OrderStatuses)))
            {
                string displayName = status switch
                {
                    OrderStatuses.Pending => "در انتظار تایید",
                    OrderStatuses.Approved => "تایید شده",
                    OrderStatuses.Canceled => "رد شده",
                    _ => status.ToString()
                };

                statuses.Add(new OrderStatusViewModel(((int)status).ToString(), displayName));
            }

            return statuses;
        }

        public void ChangeStatus(int id, OrderStatuses status)
        {
            var quary = _repositoryOrder.GetBy(id);
            if (quary == null)
                throw new NullReferenceException();

            quary.ChangeStatus(status);
            var items = _repositoryOrderItem.GetAllBy(quary.Id);
            if (quary.OrderStatus == OrderStatuses.Approved)
            {
                var commandTransaction = new CreateFinancialTransactionDto
                {
                    FinancialTransactionsCriteria = new FinancialTransactionCriteria
                    {
                        OrderId = id,
                        TransactionType = TransactionTypes.Sale,
                        Mount = quary.FinalAmount,
                    }
                };
                foreach (var item in items)
                {
                    _applicationProductItem.ChangeStatus(item.ProductItem.Id, ProductItemStatuses.Selled);
                }
                _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Mount);
                _applicationFinancialTransaction.Create(commandTransaction);
            }
            else if (quary.OrderStatus == OrderStatuses.Canceled)
            {
                foreach(var item in items)
                {
                    _applicationProductItem.ChangeStatus(item.ProductItem.Id, ProductItemStatuses.Approved);
                }
            }
            _repositoryOrder.SaveChange();
        }
    }
}
