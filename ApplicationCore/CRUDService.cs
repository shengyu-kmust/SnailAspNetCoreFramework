using ApplicationCore.Abstract;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;
using Utility.Page;
namespace ApplicationCore
{
    public abstract class CRUDService<TEntity, Source, TKey>:ICRUDService<TEntity, Source, TKey> where TEntity : IEntityId<TKey>
    {
        protected IMapper _mapper;
        protected IQueryable<Source> _querySource;
        protected IRepository<TEntity> _repository;
        public CRUDService(IMapper mapper, IRepository<TEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public TEntity Add<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>
        {
            if (saveDto.Id.Equals(default(TKey)))
            {
                saveDto.Id = IdGenerator.GeneratorId<TKey>();
            }
            _repository.Add(_mapper.Map<TEntity>(saveDto));
            return _repository.Find(saveDto.Id);
        }
        public void Delete(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }
            _repository.Delete(id);
        }
        public TEntity Update<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>
        {
            if (saveDto.Id.Equals(default(TKey)))
            {
                throw new Exception("修改时，必须转入id");
            }
            var entity = _repository.Find(saveDto.Id);
            if (entity == null)
            {
                throw new Exception("要修改的实体不存在");
            }
            _repository.Update(entity);
            return entity;
        }
        public abstract void InitQuerySource();

        public PageResult<TResult> QueryPage<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class where TQueryDto : IPagination
        {
            var query = BuildPredicateAndSelector<TResult, TQueryDto>(queryDto);
            queryDto.PageIndex = queryDto.PageIndex == 0 ? 1 : queryDto.PageIndex;
            queryDto.PageSize = queryDto.PageSize == 0 ? 25 : queryDto.PageSize;
            query = (queryDto.PageIndex <= 1) ? query.Take(queryDto.PageSize) : query.Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).Take(queryDto.PageSize);
            var items = query.AsNoTracking().ToList();
            var total = query.AsNoTracking().Count();
            return new PageResult<TResult>
            {
                Items = items,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Total = total
            };
        }
        public List<TResult> Query<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class
        {
            return BuildPredicateAndSelector<TResult, TQueryDto>(queryDto).ToList();
        }

        private IQueryable<TResult> BuildPredicateAndSelector<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class
        {
            IQueryable<Source> source = _querySource;
            var p1= ((IPredicateConvert <TQueryDto,Source>)queryDto).GetExpression(); 
            var p2= SimpleEntityExpressionGenerator.GenerateAndExpressionFromDto<Source>(queryDto);
            var s1 = _querySource.Where(p1);
            var s2 = _querySource.Where(p2);
            // 查询条件
            if (queryDto is IPredicateConvert<TQueryDto, Source> predicateConvert)
            {
                var predicate = predicateConvert.GetExpression();
                source = source.Where(predicate);
            }
            else
            {
                var predicate = SimpleEntityExpressionGenerator.GenerateAndExpressionFromDto<Source>(queryDto);
                source = source.Where(predicate);
            }
            IQueryable<TResult> result;
            // 结果映射
            if (queryDto is ISelectorBuilder<Source, TResult> selectorBuilder)
            {
                result = source.Select(selectorBuilder.GetSelector());
            }
            else
            {
                result = _mapper.ProjectTo<TResult>(source);
            }
            return result.AsNoTracking();
        }

    }
}
