namespace ERP.Application.Contract.FilterAgg
{
    public class FilterParamsDto
    {
        public int PageId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public int PageCount { get; set; }
        public string? Subject { get; set; }

        public FilterParamsDto(FilterParamsCriteria filterParamsCrieria)
        {
            PageId = filterParamsCrieria.PageId;
            Take = filterParamsCrieria.Take;
            Skip = (filterParamsCrieria.PageId - 1) * filterParamsCrieria.Take;
            PageCount = filterParamsCrieria.PageCount;
            Subject = filterParamsCrieria.Subject;
        }
    }

    public class FilterParamsCriteria
    {
        public int PageId { get; set; }
        public int Take { get; set; }
        public int PageCount { get; set; }
        public string? Subject { get; set; }
    }
}
