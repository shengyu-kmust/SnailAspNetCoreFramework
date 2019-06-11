using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    /// <summary>
    /// 审计字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityAudit<T>
    {
        T CreaterId { get; set; }
        DateTime CreateTime { get; set; }
        T UpdaterId { get; set; }
        DateTime UpdateTime { get; set; }
    }
}
