namespace ERP.Application.Contract.ProductCategoryAgg
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string CreationTime { get; set; }
        public int ProductCategoryCode { get; set; }
        public string Name { get; set; }
    }
}
