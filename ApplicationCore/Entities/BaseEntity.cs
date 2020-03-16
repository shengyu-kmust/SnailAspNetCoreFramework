using Snail.Core.Entity;
using System;

namespace ApplicationCore.Entity
{
    
    public class BaseEntity : IEntityId<string>, IEntityAudit<string>, IEntitySoftDelete
    {
        public string Id { get;set;}
        public string Creater { get;set;}
        public DateTime CreateTime { get;set;}
        public string Updater { get;set;}
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
