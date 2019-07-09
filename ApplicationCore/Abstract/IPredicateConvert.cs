using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface IPredicateConvert<TDto,TSource>
    {
        Expression<Func<TSource, bool>> GetExpression();
    }
}
