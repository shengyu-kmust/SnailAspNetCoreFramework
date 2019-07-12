using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface ISelectorBuilder<TSource, TResult>
    {
        Expression<Func<TSource, TResult>> GetSelector();

    }
}
