using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface IEntityCache2<TEntity,TCacheItem> where TEntity:IBaseEntity
    {
        List<TCacheItem> GetAll();
        void Reload();
        void AddOrUpdate(params object[] keyValues);
        Func<TEntity, TCacheItem> Convert { get; set; }
    }
    public interface IEntityCache2<TEntity> : IEntityCache2<TEntity, TEntity> where TEntity:IBaseEntity
    {

    }

    public class DefaultEntityCache<TEntity, TCacheItem> : IEntityCache2<TEntity, TCacheItem> where TEntity : IBaseEntity
    {
        private IRepository<TEntity> _repository;
        public DefaultEntityCache(IRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public Func<TEntity, TCacheItem> Convert { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddOrUpdate(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public List<TCacheItem> GetAll()
        {
            return null;
        }

        public void Reload()
        {
            
        }
    }
}
