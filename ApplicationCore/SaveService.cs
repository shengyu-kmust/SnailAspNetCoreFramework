using ApplicationCore.Abstract;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore
{
    public abstract class SaveService<TEntity, TKey> : ISaveService<TEntity, TKey> where TEntity : IEntityId<TKey> 
    {
        protected IMapper _mapper;
        protected IRepository<TEntity> _repository;
        public SaveService(IMapper mapper,IRepository<TEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public TEntity Add<TSaveDto>(TSaveDto saveDto) where TSaveDto :IIdField<TKey>
        {
            if (saveDto.Id==default)
            {
                saveDto.Id = IdGenerator.GeneratorId<TKey>();
            }
            _repository.Add(_mapper.Map<TEntity>(saveDto));
            return _repository.Find(saveDto.Id);
        }
        public void Delete(object id)
        {
            if (id==null)
            {
                throw new ArgumentNullException();
            }
            _repository.Delete(id);
        }
        public TEntity Update<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>
        {
            if (saveDto.Id==default)
            {
                throw new Exception("修改时，必须转入id");
            }
            var entity=_repository.Find(saveDto.Id);
            if (entity==null)
            {
                throw new Exception("要修改的实体不存在");
            }
            _repository.Update(entity);
            return entity;
        }
    }
}
