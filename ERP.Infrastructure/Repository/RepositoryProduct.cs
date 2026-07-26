using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryProduct : IRepositoryProduct
    {
        private readonly ERPContext _context;
        public RepositoryProduct(ERPContext context)
        {
            _context = context;
        }

        public void Create(ProductModel product)
        {
            _context.Products.Add(product);
        }

        public List<ProductModel> GetAll() 
        {
            return _context.Products.Include(x => x.ProductCateory)
                .AsNoTracking().ToList();
        }

        public ProductModel GetBy(int id)
        {
            return _context.Products.Include(x => x.ProductCateory).FirstOrDefault(x => x.Id == id);
        }

        public int CalculateCode(int categoryCode, int baseCode)
        {
            int multiplier = 1;
            int temp = baseCode;
            while(temp > 0)
            {
                multiplier *= 10;
                temp /= 10;
            }

            return (categoryCode * multiplier) + baseCode;
        }

        public bool IsExist(int id)
        {
            return _context.Products.Any(x => x.Id == id);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
