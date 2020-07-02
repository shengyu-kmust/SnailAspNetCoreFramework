using Snail.Core.Entity;

namespace ApplicationCore.Dtos
{
    public class BaseQueryPaginationDto : Pagination, IIdField<string>, IDto
    {
        public string Id { get;set;}
    }
}
