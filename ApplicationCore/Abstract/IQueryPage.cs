using Utility.Page;

namespace ApplicationCore.Abstract
{
    public interface IQueryPage<T,TResult> :IQuery<T,TResult>,IPagination where T:IBaseEntity
    {

    }
}
