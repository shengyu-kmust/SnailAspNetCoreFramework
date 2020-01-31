using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    /// <summary>
    /// 事件订阅类要实现的接口
    /// </summary>
    /// <remarks>
    /// * 为什么不直接用ICapSubscribe？，因为在Cap和autofac的interceptor一起用时，会出错。autofac在对接口进行拦截时
    /// </remarks>
    public interface ISnailCapSubscribe
    {
    }
}
