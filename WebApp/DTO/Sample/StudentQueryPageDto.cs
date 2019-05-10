using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Page;

namespace DAL.DTO.Sample
{
    public class StudentQueryPageDto : StudentQueryDto, IPagination
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
