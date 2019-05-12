namespace DAL.Services
{
    public class CRUDService<T> where T : BaseEntity
    {
        private IRepository<T> repository;
        public List<TResult> Query(IQueryDto<T> query)
        {
            var predicate = ((IQueryDto<T>)query).GeneratePredicateExpression();
            var includeFunc = ((IQueryDto<T>)query).IncludeFunc();
            var order = ((IQueryDto<T>)query).OrderFunc();
            var selector = ((IQueryDto<T>)query).SelectorExpression;
            //repository.
        }

        public PageResult<TResult> Query(IQueryDto<T> query)
        {
            var predicate = ((IQueryDto<T>)query).GeneratePredicateExpression();
            var includeFunc = ((IQueryDto<T>)query).IncludeFunc();
            var order = ((IQueryDto<T>)query).OrderFunc();
            //repository.Query
        }

    }
}
