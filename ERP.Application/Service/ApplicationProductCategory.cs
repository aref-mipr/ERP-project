using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using System.Globalization;

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
            var categories = _repositoryProductCategory.GetAll();
            var persianDate = new PersianCalendar();

            return categories.OrderByDescending(x => x.ProductCategoryCode)
                .Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    CreationTime =
                        $"{x.CreationTime:HH:mm} , " +
                        $"{persianDate.GetYear(x.CreationTime):0000}/" +
                        $"{persianDate.GetMonth(x.CreationTime):00}/" +
                        $"{persianDate.GetDayOfMonth(x.CreationTime):00}",
                    Name = x.Name,
                    ProductCategoryCode = x.ProductCategoryCode,
                }).ToList();
        }

        public List<ProductCategoryViewModel> GetAll(FilterParamsDto filterParams)
        {
            var categories = _repositoryProductCategory.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                categories = categories
                    .Where(x => x.Name.Contains(filterParams.Subject) ||
                    x.ProductCategoryCode.ToString().Contains(filterParams.Subject));

            var persianDate = new PersianCalendar();

            return categories.OrderByDescending(x => x.ProductCategoryCode)
                .Skip(filterParams.Skip)
                .Take(filterParams.Take)
                .Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                IsActive = x.IsActive,
                CreationTime =
                    $"{x.CreationTime:HH:mm} , " +
                    $"{persianDate.GetYear(x.CreationTime):0000}/" +
                    $"{persianDate.GetMonth(x.CreationTime):00}/" +
                    $"{persianDate.GetDayOfMonth(x.CreationTime):00}",
                    Name = x.Name,
                ProductCategoryCode = x.ProductCategoryCode,
            }).ToList();
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

        public int GetCount(string? subject = null)
        {
            var categories = _repositoryProductCategory.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(subject))
                categories = categories
                    .Where(x => x.Name.Contains(subject) ||
                    x.ProductCategoryCode.ToString().Contains(subject));

            return categories.Count();
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
