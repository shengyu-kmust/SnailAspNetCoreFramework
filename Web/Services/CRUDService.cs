using CommonAbstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Utility.Page;

namespace Web.Services
{
    public class CRUDService<T> where T : IBaseEntity
    {
        private IRepository<T> _repository;
        public CRUDService(IRepository<T> repository)
        {
            _repository = repository;
        }
        public List<TResult> Query<TResult>(IQueryDto<T> query, Expression<Func<T, TResult>>  selector)
        {
            var predicate = query.GeneratePredicateExpression();
            var includeFunc = query.IncludeFunc();
            var order = query.OrderFunc();
            return _repository.Query(predicate, includeFunc, order, selector);
        }

        public PageResult<TResult> QueryPage<TResult>(IQueryPageDto<T> query, Expression<Func<T, TResult>> selector)
        {
            var predicate = query.GeneratePredicateExpression();
            var includeFunc = query.IncludeFunc();
            var order = query.OrderFunc();
            return _repository.QueryPage(predicate, includeFunc, order, new DefaultPagination() { PageIndex = query.PageIndex, PageSize = query.PageIndex }, selector);
        }


    }
}
