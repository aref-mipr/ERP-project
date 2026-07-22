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
            command.ProductItemCriterias.Price = product.SellPrice;
            int baseProductItemCode = _repositoryProductItem.GetAll().Where(x => x.ProductId == command.ProductItemCriterias.ProductId).Count();
            command.ProductItemCriterias.ProductItemCode = _repositoryProductItem.CalculateCode(product.ProductCode, baseProductItemCode);
            var productItem = new ProductItemModel(command.ProductItemCriterias);
            _repositoryProductItem.Create(productItem);
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
                        Mount = quary.Product.CostPrice,
                    }
                };
                _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Mount);
                _applicationFinancialTransaction.Create(commandTransaction);
            }
            _repositoryProductItem.SaveChange();
        }

        public ProductItemViewModel GetBy(long id)
        {
            var quary = _repositoryProductItem.GetBy(id);
            var productItem = new ProductItemViewModel
            {
                Id = quary.Id,
                Name = quary.Product.Name,
                Category = quary.Product.ProductCateory.Name,
                CreationTime = quary.Product.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                ProductItemStatus = _enumExtension.ItemStatusesToPersianString(quary.ProductItemStatus),
                ProductItemCriterias = new ProductItemCriteria
                {
                    ProductId = quary.ProductId,
                    ProductItemCode = quary.ProductItemCode,
                    Price = quary.Price,
                    Description = quary.Description,
                }
            };

            return productItem;
        }
        public List<ProductItemViewModel> GetAll()
        {
            return _repositoryProductItem.GetAll()
                .Select(x => new ProductItemViewModel{
                    Id = x.Id,
                    Name = x.Product.Name,
                    CreationTime = x.Product.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                    ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus),
                    ProductItemCriterias = new ProductItemCriteria
                    {
                        ProductId = x.ProductId,
                        ProductItemCode = x.ProductItemCode,
                        Price = x.Price,
                        Description = x.Description,
                    }
            }).OrderBy(x => x.ProductItemCriterias.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetAllReadyToSell()
        {
            return _repositoryProductItem.GetAllReadyToSell()
                .Select(x => new ProductItemViewModel{
                    Id = x.Id,
                    Name = x.Product.Name,
                    CreationTime = x.Product.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                    ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus),
                    ProductItemCriterias = new ProductItemCriteria
                    {
                        ProductId = x.ProductId,
                        ProductItemCode = x.ProductItemCode,
                        Price = x.Price,
                        Description = x.Description,
                    }
            }).OrderBy(x => x.ProductItemCriterias.ProductItemCode).ToList();
        }

        public List<ProductItemViewModel> GetAllBy(int productId)
        {
            return _repositoryProductItem.GetAllBy(productId)
                .Select(x => new ProductItemViewModel
                {
                    Id = x.Id,
                    Name = x.Product.Name,
                    ProductItemStatus = _enumExtension.ItemStatusesToPersianString(x.ProductItemStatus),
                    ProductItemCriterias = new ProductItemCriteria
                    {
                        ProductId = x.ProductId,
                        ProductItemCode = x.ProductItemCode,
                        Price = x.Price,
                    }
                }).OrderBy(x => x.ProductItemCriterias.ProductItemCode).ToList();
           
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
