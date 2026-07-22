using ERP.Domain.Criteria;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Application.Contract.ProductCategoryAgg
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public ProductCategoryCriteria ProductCategoryCriterias { get; set; }
        public bool IsActive { get; set; }
        public string CreationTime { get; set; }
        public List<string> ProductsName { get; set; }
    }
}
