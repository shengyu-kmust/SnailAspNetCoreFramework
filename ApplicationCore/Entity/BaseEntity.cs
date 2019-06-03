using ApplicationCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entity
{
    /// <summary>
    /// 实体的公共属性，如ID，创建时间，最后一次更新时间，是否有效
    /// </summary>
    public class BaseEntity : IEntityId<int>, IEntityAudit<int>, IEntitySoftDelete
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //[Key]
        //public int Id { get; set; }
        //public DateTime CreateTime { get; set; }
        //public DateTime UpdateTime { get; set; }=DateTime.Now;
        //public int IsValid { get; set; } = 1;
        public int Id { get;set;}
        public int CreaterId { get;set;}
        public DateTime CreateTime { get;set;}
        public int UpdaterId { get;set;}
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
