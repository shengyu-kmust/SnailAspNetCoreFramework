using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface IEntityAudit<T>
    {
        T CreaterId { get; set; }
        DateTime CreateTime { get; set; }
        T UpdaterId { get; set; }
        DateTime UpdateTime { get; set; }
    }
}
