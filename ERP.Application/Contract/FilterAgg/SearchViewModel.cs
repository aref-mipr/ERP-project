using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.Contract.FilterAgg
{
    public class SearchViewModel
    {
        public FilterParamsDto FilterParams { get; set; }
        public Dictionary<string, string[]>? AdditionalParameters { get; set; } = new();
    }
}
