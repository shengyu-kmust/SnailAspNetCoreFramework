using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Dtos
{
    /// <summary>
    ///  todo:IPagenation默认实现
    /// </summary>
    public class KeyQueryDto: PaginationQueryDto
    {
        public string KeyWord { get; set; }
    }
}
