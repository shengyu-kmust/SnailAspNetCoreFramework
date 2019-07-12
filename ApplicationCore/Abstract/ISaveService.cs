using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface ISaveService<TEntity,TKey> 
    {
        TEntity Add<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>;
        TEntity Update<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>;
        void Delete(object id);
    }
}
