using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApp.DTO
{
    /// <summary>
    /// 所有的查询条件
    /// </summary>
    public interface IQueryDto<T>:IValidatableObject where T:BaseEntity
    {
        Expression<Func<T,bool>> GeneratePredicateExpression();
        Func<IQueryable<T>, IQueryable<T>> IncludeFunc();
        Func<IQueryable<T>, IQueryable<T>> OrderFunc();
        Expression<Func<T, TResult>> SelectorExpression<TResult>();
    }
}
