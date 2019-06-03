using Utility.Page;

namespace ApplicationCore.Abstract
{
    public interface IQueryPageDto<T> :IQueryDto<T>,IPagination where T:IBaseEntity
    {

    }
}
