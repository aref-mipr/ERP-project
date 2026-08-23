using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using System.Globalization;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Application.Service
{
    public class ApplicationProductItem: IApplicationProductItem
    {
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IRepositoryOrderItem _repositoryOrderItem;
        private readonly IRepositoryProduct _repositoryProduct;
        private readonly IEnumExtension _enumExtension;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationBudget _applicationBudget;
        public ApplicationProductItem(IRepositoryProductItem repositoryProductItem, IRepositoryProduct repositoryProduct,
            IEnumExtension enumExtension, IRepositoryOrderItem repositoryOrderItem,
            IApplicationFinancialTransaction applicationFinancialTransaction, IApplicationBudget applicationBudget)
        {
            _repositoryProductItem = repositoryProductItem;
            _repositoryOrderItem = repositoryOrderItem;
            _repositoryProduct = repositoryProduct;
            _enumExtension = enumExtension;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationBudget = applicationBudget;
        }

        public void Create(CreateProductItemDto command)
        {
            var product = _repositoryProduct.GetBy(command.ProductItemCriterias.ProductId);
            int baseProductItemCode = _repositoryProductItem.GetAll().Where(x => x.ProductId == command.ProductItemCriterias.ProductId).Count()+1;
            command.ProductItemCriterias.ProductItemCode = _repositoryProductItem.CalculateCode(product.ProductCode, baseProductItemCode);
            var productItem = new ProductItemModel(command.ProductItemCriterias);
            _repositoryProductItem.Create(productItem);
            _repositoryProductItem.SaveChange();

            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    ProductItemId = productItem.Id,
                    ProductId = productItem.ProductId,
                    Amount = -productItem.Product.CostPrice,
                    TransactionType = TransactionTypes.Purchase,
                }
            };
            _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
            _applicationFinancialTransaction.Create(commandTransaction);
            _repositoryProductItem.SaveChange();
        }
        public void Edit(EditProductItemDto command)
        {
            var quary = _repositoryProductItem.GetBy(command.Id);
            if (quary == null)
                throw new NullReferenceException();

            quary.Edit(command.ProductItemCriterias);
            if (quary.ProductItemStatus == ProductItemStatuses.Returned)
            {
                var commandTransaction = new CreateFinancialTransactionDto
                {
                    FinancialTransactionsCriteria = new FinancialTransactionCriteria
                    {
                        ProductItemId = command.Id,
                        TransactionType = TransactionTypes.ReturnedProduct,
                        Amount = quary.Product.CostPrice,
                        ProductId = quary.ProductId,
                    }
                };
                _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
                _applicationFinancialTransaction.Create(commandTransaction);
            }
            _repositoryProductItem.SaveChange();
        }

        public ProductItemViewModel GetBy(long id)
        {
            var quary = _repositoryProductItem.GetBy(id);
            var persianDate = new PersianCalendar();

            return new ProductItemViewModel
            {
                Id = quary.Id,
                Name = quary.Product.Name,
                Category = quary.Product.ProductCateory.Name,
                CreationTime =
                    $"{quary.Product.CreationTime:HH:mm} , " +
                    $"{persianDate.GetYear(quary.Product.CreationTime):0000}/" +
                    $"{persianDate.GetMonth(quary.Product.CreationTime):00}/" +
                    $"{persianDate.GetDayOfMonth(quary.Product.CreationTime):00}",
                ProductItemStatus = _enumExtension.ItemStatusesToPersianString(quary.ProductItemStatus),
                ProductId = quary.ProductId,
                ProductItemCode = quary.ProductItemCode,
                Price = quary.Price,
                Description = quary.Description,
            };
        }

        public CreateProductItemDto GetBy(int productId)
        {
            var product = _repositoryProduct.GetBy(productId);
            
            return new CreateProductItemDto
            {
                ProductItemCriterias = new ProductItemCriteria
                {
                    ProductId = product.Id,
                }
            };
        }

        public List<ProductItemViewModel> GetAll()
        {
            return _repositoryProductItem.GetAll()
                .Select(x => new ProductItemViewModel{
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Name = x.Product.Name,
                    ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus),
                    ProductItemCode = x.ProductItemCode,
                    Price = x.Price,
            }).OrderByDescending(x => x.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetAllReadyToSell(int id)
        {
            var quary = _repositoryProductItem.GetAll()
                .Where(x => x.ProductItemStatus == ProductItemStatuses.Approved).AsQueryable();

            if (id != 0)
            {
                var itemsReadyToSell = quary.ToList();
                var orderItems = _repositoryOrderItem.GetAllBy(id);
                foreach (var orderIrem in orderItems)
                {
                    var itemInOrder = _repositoryProductItem.GetBy(orderIrem.ProductItemId);
                    itemsReadyToSell.Add(itemInOrder);
                }
                quary = itemsReadyToSell.AsQueryable();
            }

            return quary.OrderByDescending(x => x.ProductItemCode)
                .Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                Name = x.Product.Name,
                ProductItemCode = x.ProductItemCode,
            }).ToList();
        }

        public List<ProductItemViewModel> GetAllBy(int productId)
        {
            var quary = _repositoryProductItem.GetAll().Where(x => x.ProductId == productId);
            return quary.Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                ProductItemCode = x.ProductItemCode,
                Price = x.Price,
                ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus)
            }).OrderByDescending(x => x.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetIAlltemsInWarehouse(FilterParamsDto filterParams)
        {
            var quary = _repositoryProductItem.GetAll()
                .Where(x => x.ProductItemStatus == ProductItemStatuses.Testing
                    || x.ProductItemStatus == ProductItemStatuses.Approved
                    || x.ProductItemStatus == ProductItemStatuses.Unsellable
                    || x.ProductItemStatus == ProductItemStatuses.WaitingOrder).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                quary = quary.Where(x => x.Product.Name.Contains(filterParams.Subject));

            return quary.OrderByDescending(x => x.ProductItemCode)
                .Skip(filterParams.Skip)
                .Take(filterParams.Take)
                .Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                Name = x.Product.Name,
                ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus),
                ProductId = x.ProductId,
                ProductItemCode = x.ProductItemCode,
                Price = x.Price,
            }).ToList();
        }

        public EditProductItemDto GetForEdit(long id)
        {
            var productItem = _repositoryProductItem.GetBy(id);
            return new EditProductItemDto
            {
                Id = productItem.Id,
                ProductItemCriterias = new ProductItemCriteria
                {
                    Price = productItem.Price,
                    Description = productItem.Description,
                    ProductItemStatus = productItem.ProductItemStatus,
                }
            };
        }

        public int GetCount(string? subject = null)
        {
            var customers = _repositoryProductItem.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(subject))
                customers = customers.Where(x => x.Product.Name.Contains(subject));

            return customers.Count();
        }

        public int GetCountInWarehouse(string? subject = null)
        {
            var customers = _repositoryProductItem.GetAll()
                .Where(x => x.ProductItemStatus == ProductItemStatuses.Testing
                    || x.ProductItemStatus == ProductItemStatuses.Approved
                    || x.ProductItemStatus == ProductItemStatuses.Unsellable
                    || x.ProductItemStatus == ProductItemStatuses.WaitingOrder).AsQueryable();

            if (!string.IsNullOrWhiteSpace(subject))
                customers = customers.Where(x => x.Product.Name.Contains(subject));

            return customers.Count();
        }

        public void ChangeStatus(long id, ProductItemStatuses status)
        {
            var quary = _repositoryProductItem.GetBy(id);
            quary.ChangeStatus(status);
        } 

        public List<ProductItemStatusViewModel> CreateStatuses()
        {
            var statuses = new List<ProductItemStatusViewModel>();

            foreach (ProductItemStatuses status in Enum.GetValues(typeof(ProductItemStatuses)))
            {
                string displayName = status switch
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

                statuses.Add(new ProductItemStatusViewModel(((int)status).ToString(), displayName));
            }

            return statuses;
        }
    }
}
