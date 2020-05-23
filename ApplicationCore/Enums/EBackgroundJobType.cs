using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Enums
{
    public enum EBackgroundJobType
    {
        /// <summary>
        /// 立即执行，并只执行一次
        /// </summary>
        Enqueue,
        /// <summary>
        /// 多久后执行，并执行一次
        /// </summary>
        Schedule,
        /// <summary>
        /// 循环执行
        /// </summary>
        Recurring
    }
}
