using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.DataDictionary
{
    public class DataDictionaryQueryDto : IPagination
    {
        public int PageSize { get;set;}
        public int PageIndex { get;set;}
    }
}
