using Snail.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Dtos
{
    /// <summary>
    /// todo:抽到snail
    /// </summary>
    public class Pagination : IPagination
    {
        public int PageSize { get; set; } = 15;
        public int PageIndex { get; set; } = 1;
    }
}
