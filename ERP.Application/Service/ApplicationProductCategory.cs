using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;

namespace ERP.Application.Service
{
    public class ApplicationProductCategory : IApplicationProductCategory
    {
        private readonly IRepositoryProductCategory _repositoryProductCategory;
        public ApplicationProductCategory(IRepositoryProductCategory repositoryProductCategory)
        {
            _repositoryProductCategory = repositoryProductCategory;
        }
        public void Create(CreateProductCategoryDto command)
        {
            command.ProductCategoryCriterias.ProductCategoryCode = _repositoryProductCategory.GetAll().Count() + 10;
            var productCategory = new ProductCategoryModel(command.ProductCategoryCriterias);
            _repositoryProductCategory.Create(productCategory);
            _repositoryProductCategory.SaveChange();
        }

        public void Edit(EditProductCategoryDto command)
        {
            var quary = _repositoryProductCategory.GetBy(command.Id);
            if (quary == null)
                throw new NullReferenceException();

            quary.Edit(command.ProductCategoryCriterias.Name);
            _repositoryProductCategory.SaveChange();
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _repositoryProductCategory.GetAll().Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                IsActive = x.IsActive,
                CreationTime = x.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                Name = x.Name,
                ProductCategoryCode = x.ProductCategoryCode,
            }).OrderByDescending(x => x.ProductCategoryCode).ToList();
        }

        public EditProductCategoryDto GetForEdit(int id)
        {
            var productCategory = _repositoryProductCategory.GetBy(id);
            if (productCategory == null)
                throw new NullReferenceException();

            return new EditProductCategoryDto
            {
                Id = productCategory.Id,
                ProductCategoryCriterias = new ProductCategoryCriteria { Name = productCategory.Name, }
            };
        }

        public void Remove(int id)
        {
            var quary = _repositoryProductCategory.GetBy(id);
            if (quary == null)
                throw new NullReferenceException();

            quary.Remove();
            _repositoryProductCategory.SaveChange();
        }

        public void Restore(int id)
        {
            var quary = _repositoryProductCategory.GetBy(id);
            if (quary == null)
                throw new NullReferenceException();

            quary.Restore();
            _repositoryProductCategory.SaveChange();
        }
    }
}
