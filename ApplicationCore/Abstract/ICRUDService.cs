using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Utility.Page;

namespace ApplicationCore.Abstract
{
    public interface ICRUDService<TEntity,Source> where TEntity:IBaseEntity
    {
        void SetQuerySource(IQueryable<Source> querySource);
        PageResult<TResult> QueryPage<TResult,TQueryDto>(TQueryDto queryDto) where TQueryDto : IPagination;
        List<TResult> Query<TResult,TQueryDto>(TQueryDto queryDto);
        TResult Single<TResult>(object id);

        TResult Add<TResult, TSaveDto>(TSaveDto save);
        void Add<TSaveDto>(TSaveDto saveDto);
        TResult Update<TResult, TSaveDto>(TSaveDto save);
        void Update<TSaveDto>(TSaveDto saveDto);
        void Delete(object id);
    }

    public interface ICRUDService<TEntity> : ICRUDService<TEntity, TEntity> where TEntity : IBaseEntity
    {
    }

}
