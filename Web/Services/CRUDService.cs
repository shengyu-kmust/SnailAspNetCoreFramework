using ApplicationCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Utility.Page;

namespace Web.Services
{
    /// <summary>
    /// 通用CRUD服务，做为
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CRUDService<T> where T : IBaseEntity,new()
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

        public void Update(ISaveDto<T> saveDto)
        {
            var entity = saveDto.ConvertToEntity();
            var changeProperties = saveDto.GetUpdateProperties();
            _repository.Update(entity, changeProperties);
        }

        public void Add(ISaveDto<T> saveDto)
        {
            var entity= saveDto.ConvertToEntity();
            _repository.Add(entity);
        }

        public void Delete(params object[] keyValues)
        {
            _repository.Delete(keyValues);
        }
    }
}
