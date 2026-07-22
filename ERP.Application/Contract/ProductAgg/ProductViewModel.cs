using ERP.Domain.Criteria;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Application.Contract.ProductAgg
{
    public class ProductViewModel
    {
        public int Id { get; set; } 
        public ProductCriteria ProductCriterias { get; set; }
        public string CreationTime { get; set; }
        public string ProductCategory { get; set; }
    }
}
