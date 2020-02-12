using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Enums
{
    /// <summary>
    /// 字典配置的操作类型
    /// </summary>
    public enum EDataDictionaryOperType
    {
        /// <summary>
        /// 能进行全部的操作
        /// </summary>
        All,
        /// <summary>
        /// 只能修改值
        /// </summary>
        OnlyChangeValue,
        /// <summary>
        /// 不能进行所有的操作
        /// </summary>
        None,
    }
}
