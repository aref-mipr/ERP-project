using System.ComponentModel.DataAnnotations;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Application.Contract.ProductItemAgg
{
    public class ProductItemViewModel: CreateProductItemDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string CreationTime { get; set; }
        public string ProductItemStatus { get; set; }

    }
}