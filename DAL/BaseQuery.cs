using Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Web
{
    public class BaseQuery<T> : IQuery<T>
    {
        public virtual Expression<Func<T, bool>> GeneratePredicate()
        {
            throw new NotImplementedException();
        }
    }
}
