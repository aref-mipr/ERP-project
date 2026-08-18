using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using static ERP.Domain.Entity.FinancialTransactionModel;

namespace ERP.Application.Service
{
    public class ApplicationProduct : IApplicationProduct
    {
        private readonly IRepositoryProduct _repositoryProduct;
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IRepositoryProductCategory _repositoryProductCategory;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationBudget _applicationBudget;
        private readonly IRepositoryBudget _repositoryBudget;
        public ApplicationProduct(IRepositoryProduct repositoryProduct, IRepositoryProductCategory repositoryProductCategory,
            IRepositoryProductItem repositoryProductItem, IApplicationProductItem applicationProductItem,
            IApplicationFinancialTransaction applicationFinancialTransaction, IApplicationBudget applicationBudget, IRepositoryBudget repositoryBudget)
        {
            _repositoryProduct = repositoryProduct;
            _repositoryProductCategory = repositoryProductCategory;
            _repositoryProductItem = repositoryProductItem;
            _applicationProductItem = applicationProductItem;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationBudget = applicationBudget;
            _repositoryBudget = repositoryBudget;
        }

        public void Create(CreateProductDto command)
        {
            if((command.ProductCriterias.CostPrice * command.ProductCriterias.StockQuantity) > _repositoryBudget.GetLast().TotalBudget)
                throw new ArgumentOutOfRangeException();

            var category = _repositoryProductCategory.GetBy(command.ProductCriterias.ProductCategoryId);
            int categoryCode = category.ProductCategoryCode;
            int baseCode = category.Products.Count() + 1001;
            command.ProductCriterias.ProductCode = _repositoryProduct.CalculateCode(categoryCode, baseCode);
            var product = new ProductModel(command.ProductCriterias);
            _repositoryProduct.Create(product);
            _repositoryProduct.SaveChange();

            int stockQuantityTemp = product.StockQuantity;
            while (stockQuantityTemp > 0)
            {
                command.ProductItemCriterias = new ProductItemCriteria();
                command.ProductItemCriterias.ProductId = product.Id;
                command.ProductItemCriterias.Price = product.SellPrice;
                int baseProductItemCode = stockQuantityTemp;
                command.ProductItemCriterias.ProductItemCode = _repositoryProductItem.CalculateCode(product.ProductCode, baseProductItemCode);
                var productItem = new ProductItemModel(command.ProductItemCriterias);
                _repositoryProductItem.Create(productItem);
                stockQuantityTemp--;
            }

            if (_repositoryBudget.GetLast().TotalBudget < (product.StockQuantity * product.CostPrice))
                throw new NullReferenceException();

            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    ProductId = product.Id,
                    TransactionType = TransactionTypes.Purchase,
                    Amount = -(product.StockQuantity * product.CostPrice),
                }
            };
            _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
            _applicationFinancialTransaction.Create(commandTransaction);
            _repositoryProduct.SaveChange();
        }

        public void Edit(EditProductDto command)
        {
            var quary = _repositoryProduct.GetBy(command.Id);
            if (quary == null)
                throw new NullReferenceException();

            command.ProductCriterias.SellPrice = quary.SellPrice;

            if (command.ProductCriterias.CostPrice != quary.CostPrice)
            {
                var commandTransaction = new CreateFinancialTransactionDto
                {
                    FinancialTransactionsCriteria = new FinancialTransactionCriteria
                    {
                        ProductId = command.Id,
                        TransactionType = TransactionTypes.Adjustment,
                        Amount = quary.StockQuantity * (quary.CostPrice - command.ProductCriterias.CostPrice),
                    }
                };
                _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
                _applicationFinancialTransaction.Create(commandTransaction);
            }
            quary.Edit(command.ProductCriterias);
            _repositoryProduct.SaveChange();
        }

        public ProductViewModel GetBy(int id)
        {
            var quary =  _repositoryProduct.GetBy(id);
            var product = new ProductViewModel
            {
                Id = quary.Id,
                CreationTime = quary.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                ProductCategory = quary.ProductCateory.Name,
                ProductCode = quary.ProductCode,
                Name = quary.Name,
                Description = quary.Description,
                CostPrice = quary.CostPrice,
                StockQuantity = quary.StockQuantity,

            };
            return product;
        }

        public List<ProductViewModel> GetAll()
        {
            return  _repositoryProduct.GetAll().Select(x => new ProductViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                ProductCategory = x.ProductCateory.Name,
                ProductCode = x.ProductCode,
                Name = x.Name,
                CostPrice = x.CostPrice,
                StockQuantity = x.StockQuantity,
            }).OrderByDescending(x => x.ProductCode).ToList();
        }

        public EditProductDto GetForEdit(int id)
        {
            var product = _repositoryProduct.GetBy(id);
            return new EditProductDto
            {
                Id = product.Id,
                ProductCriterias = new ProductCriteria
                {
                    ProductCategoryId = product.ProductCategoryId,
                    Name = product.Name,
                    CostPrice = product.CostPrice,
                    Description = product.Description,
                    StockQuantity = product.StockQuantity,
                }
            };
        }

        public List<ProductViewModel> GetProductsByCategoryId(int categoryId)
        {
            return _repositoryProduct.GetAll()
                .Where(x => x.ProductCategoryId == categoryId)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).OrderByDescending(x => x.ProductCode).ToList();
        }

        public int GetCount()
        {
            return _repositoryProduct.GetAll().Count();
        }

        public void ChangeStockQuantity(int id, int quantity)
        {
            var product = _repositoryProduct.GetBy(id);
            if (product == null)
                throw new NullReferenceException();

            product.ChangeStockQuantity(quantity);
            _repositoryProduct.SaveChange();
        }

    }
}
