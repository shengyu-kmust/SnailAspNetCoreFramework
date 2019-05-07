using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Page
{
    public class PageResult<T>:IPagination
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
