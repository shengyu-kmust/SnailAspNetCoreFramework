using ApplicationCore.Abstract;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utility;
using Utility.Page;

namespace ApplicationCore
{
    public class CRUDService<TEntity, Source> : ICRUDService<TEntity, Source> where TEntity:IBaseEntity
    {
        private IMapper _mapper;
        private IQueryable<Source> _querySource;
        public CRUDService<TEntity, Source>()
        {

        }
        public TResult Add<TResult, TSaveDto>(TSaveDto save)
        {
            throw new NotImplementedException();
        }

        public void Add<TSaveDto>(TSaveDto saveDto)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public List<TResult> Query<TResult, TQueryDto>(TQueryDto queryDto)
        {
            IQueryable<Source> source= _querySource;
            // 查询条件
            if (queryDto is IPredicateConvert<TQueryDto,Source> predicateConvert)
            {
                source=_querySource.Where(predicateConvert.GetExpression());
            }
            else
            {
                source = _querySource.Where(SimpleEntityExpressionGenerator.GenerateAndExpressionFromDto<Source>(queryDto));
            }


            // 结果映射
            throw new NotImplementedException();
        }

        public PageResult<TResult> QueryPage<TResult, TQueryDto>(TQueryDto queryDto) where TQueryDto : IPagination
        {
            throw new NotImplementedException();
        }

        public void SetQuerySource(IQueryable<Source> querySource)
        {
            _querySource = querySource;
        }

        public TResult Single<TResult>(object id)
        {
            throw new NotImplementedException();
        }

        public TResult Update<TResult, TSaveDto>(TSaveDto save)
        {
            throw new NotImplementedException();
        }

        public void Update<TSaveDto>(TSaveDto saveDto)
        {
            throw new NotImplementedException();
        }
    }

}
