using Snail.Core.Entity;
using System;

namespace ApplicationCore.Entity
{
    /// <summary>
    /// 实体的公共属性，如ID，创建时间，最后一次更新时间，是否有效
    /// </summary>
    public class BaseEntity : IEntityId<string>, IEntityAudit<string>, IEntitySoftDelete
    {
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Creater { get;set; }
        public string Updater { get;set; }
    }
}
