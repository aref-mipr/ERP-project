using ERP.Infrastructure.Context;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryProductCategory: IRepositoryProductCategory
    {
        private readonly ERPContext _context;
        public RepositoryProductCategory(ERPContext context)
        {
            _context = context;
        }

        public void Create(ProductCategoryModel category)
        {
            _context.ProductCategories.Add(category);
        }

        public List<ProductCategoryModel> GetAll()
        {
            return _context.ProductCategories.AsNoTracking().ToList();
        }

        public ProductCategoryModel GetBy(int id)
        {
            return _context.ProductCategories.Include(x => x.Products).FirstOrDefault(x => x.Id == id);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
