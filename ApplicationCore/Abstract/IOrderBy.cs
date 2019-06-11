using ApplicationCore.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface IOrderBy
    {
        /// <summary>
        /// 排序字段名
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// 升/降序
        /// </summary>
        Sort Sort { get; set; }
    }
}
