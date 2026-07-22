using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class ProductCategoryModel
    {
        public int Id { get; private set; }
        public int ProductCategoryCode { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreationTime { get; private set; }

        public ICollection<ProductModel> Products { get; private set; }

        protected ProductCategoryModel() { }

        public ProductCategoryModel(ProductCategoryCriteria categoryCriteria)
        {
            ProductCategoryCode = categoryCriteria.ProductCategoryCode;
            Name = categoryCriteria.Name;
            IsActive = true;
            CreationTime = DateTime.Now;
            Products = new List<ProductModel>();
        }

        public void Edit(string name)
        {
            Name = name;
        }

        public void Remove()
        {
            IsActive = false;
        }

        public void Restore()
        {
            IsActive = true;
        }
    }
}
