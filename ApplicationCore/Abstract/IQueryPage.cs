using Utility.Page;

namespace ApplicationCore.Abstract
{
    public interface IQueryPage<T> :IQuery<T>,IPagination where T:IBaseEntity
    {

    }
}
