using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAbstract
{
    public interface IEntityAudit<T>
    {
        T CreaterId { get; set; }
        DateTime CreateTime { get; set; }
        T UpdaterId { get; set; }
        DateTime UpdateTime { get; set; }
    }
}
