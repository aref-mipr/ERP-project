using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryProductItem: IRepositoryProductItem
    {
        private readonly ERPContext _context;
        public RepositoryProductItem(ERPContext context)
        {
            _context = context;
        }
        public void Create(ProductItemModel productItem)
        {
            _context.ProductItems.Add(productItem);
        }
        public ProductItemModel GetBy(long id)
        {
            return _context.ProductItems.Include(x => x.Product).Include(x => x.Product.ProductCateory).FirstOrDefault(x => x.Id == id);
        }

        public List<ProductItemModel> GetAll()
        {
            return _context.ProductItems.Include(x => x.Product)
              .AsNoTracking().ToList();
        }

        public int CalculateCode(int productCode, int baseCode)
        {
            int multiplier = 1;
            int temp = baseCode;
            while (temp > 0)
            {
                multiplier *= 10;
                temp /= 10;
            }

            return (productCode * multiplier) + baseCode;
        }

        public bool IsExist(long id)
        {
            return _context.ProductItems.Any(x => x.Id == id);

        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
