using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
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
            return new ProductItemViewModel
            {
                Id = quary.Id,
                Name = quary.Product.Name,
                Category = quary.Product.ProductCateory.Name,
                CreationTime = quary.Product.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
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
            }).OrderBy(x => x.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetAllReadyToSell()
        {
            var quary = _repositoryProductItem.GetAll()
                .Where(x => x.ProductItemStatus == ProductItemStatuses.Approved);

            return quary.Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                Name = x.Product.Name,
                ProductItemCode = x.ProductItemCode,
            }).OrderBy(x => x.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetAllBy(int productId)
        {
            var quary = _repositoryProductItem.GetAll().Where(x => x.ProductId == productId);
            return quary.Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                ProductItemCode = x.ProductItemCode,
                Price = x.Price,
            }).OrderBy(x => x.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetIAlltemsInWarehouse()
        {
            var quary = _repositoryProductItem.GetAll()
                .Where(x => x.ProductItemStatus == ProductItemStatuses.Testing
                    || x.ProductItemStatus == ProductItemStatuses.Approved
                    || x.ProductItemStatus == ProductItemStatuses.Unsellable
                    || x.ProductItemStatus == ProductItemStatuses.WaitingOrder);

            return quary.Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                Name = x.Product.Name,
                ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus),
                ProductId = x.ProductId,
                ProductItemCode = x.ProductItemCode,
                Price = x.Price,
            }).OrderBy(x => x.ProductItemCode).ToList();
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
