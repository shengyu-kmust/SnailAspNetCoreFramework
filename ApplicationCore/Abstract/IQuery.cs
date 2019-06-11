using System;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore.Abstract
{
    /// <summary>
    /// 所有的查询条件
    /// </summary>
    public interface IQuery<T,TResult> where T:IBaseEntity
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <remarks>
        /// 1、可由工具类，根据dto自动生成expression
        /// 2、可在方法里自己写expression，用于处理复杂的查询，如筛选条件包含在关联表里。
        /// </remarks>
        /// <returns></returns>
        Expression<Func<T,bool>> GeneratePredicateExpression();
        Func<IQueryable<T>, IQueryable<T>> IncludeFunc();
        Func<IQueryable<T>, IQueryable<T>> OrderFunc();
        Expression<Func<T, TResult>> SelectorExpression();
    }
}
