using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    /// <summary>
    /// 默认的entity,在开发时，请根据需要改变此abstract类
    /// </summary>
    public abstract class DefaultBaseEntity : IBaseEntity,IEntityId<Guid>, IEntitySoftDelete, IEntityAudit<Guid>
    {
        public Guid CreaterId { get;set;}
        public DateTime CreateTime { get;set;}
        public Guid UpdaterId { get;set;}
        public DateTime UpdateTime { get;set;}
        public bool IsDeleted { get;set;}
        public Guid Id { get;set;}
    }
} 
