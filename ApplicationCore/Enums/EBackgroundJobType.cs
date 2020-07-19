using System.ComponentModel;

namespace ApplicationCore.Enums
{
    public enum EBackgroundJobType
    {
        /// <summary>
        /// 立即执行，并只执行一次
        /// </summary>
        [Description("不能进行所有的操作")]
        Enqueue, 
        /// <summary>
        /// 多久后执行，并执行一次
        /// </summary>
        [Description("不能进行所有的操作")]
        Schedule,
        /// <summary>
        /// 循环执行
        /// </summary>
        [Description("不能进行所有的操作")]
        Recurring
    }
}
