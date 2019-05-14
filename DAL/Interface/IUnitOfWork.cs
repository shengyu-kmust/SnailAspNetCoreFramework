using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;

namespace Web.Interface
{
    public interface IUnitOfWork:IDisposable 
    {
        Task SaveAsync();
    }
}
