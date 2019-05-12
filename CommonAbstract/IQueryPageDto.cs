using Utility.Page;

namespace CommonAbstract
{
    public interface IQueryPageDto<T>:IQueryDto<T>,IPagination where T:IBaseEntity
    {

    }
}
