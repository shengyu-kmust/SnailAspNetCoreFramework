﻿using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Web.Services
{
    /// <summary>
    /// 通用CRUD服务，做为
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CRUDService<T> where T : IBaseEntity,new()
    {
        //private IRepository<T> _repository;
        //public CRUDService(IRepository<T> repository)
        //{
        //    _repository = repository;
        //}
        //public List<TResult> Query<TResult>(IQuery<T,TResult> query)
        //{
        //    var predicate = query.GeneratePredicateExpression();
        //    var includeFunc = query.IncludeFunc();
        //    var order = query.OrderFunc();
        //    var selector = query.SelectorExpression();
        //    return _repository.Query(predicate, includeFunc, order, selector);
        //}

        //public PageResult<TResult> QueryPage<TResult>(IQueryPage<T,TResult> query, Expression<Func<T, TResult>> selector)
        //{
        //    var predicate = query.GeneratePredicateExpression();
        //    var includeFunc = query.IncludeFunc();
        //    var order = query.OrderFunc();
        //    return _repository.QueryPage(predicate, includeFunc, order, new DefaultPagination() { PageIndex = query.PageIndex, PageSize = query.PageIndex }, selector);
        //}

        //public void Update(ISaveDto<T> saveDto)
        //{
        //    var entity = saveDto.ConvertToEntity();
        //    var changeProperties = saveDto.GetUpdateProperties();
        //    _repository.Update(entity, changeProperties);
        //}

        //public void Add(ISaveDto<T> saveDto)
        //{
        //    var entity= saveDto.ConvertToEntity();
        //    _repository.Add(entity);
        //}

        //public void Delete(params object[] keyValues)
        //{
        //    _repository.Delete(keyValues);
        //}
    }
}
