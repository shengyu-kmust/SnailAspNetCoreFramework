using Snail.Core;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.Sample
{
    public class SampleQueryDto : IIdField<string>,IPagination
    {
        public string Id { get; set; }
        public int PageSize { get;set; }
        public int PageIndex { get;set; }
    }
}
