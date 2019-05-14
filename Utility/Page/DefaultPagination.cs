using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Page
{
    public class DefaultPagination : IPagination
    {
        public int PageSize { get;set; }
        public int PageIndex { get;set; }
    }
}
