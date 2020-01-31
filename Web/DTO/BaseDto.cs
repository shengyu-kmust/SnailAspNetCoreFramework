using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO
{
    public class BaseDto : IDto, IIdField<string>
    {
        public string Id { get; set; }
    }

    public class BaseAuditDto: BaseDto
    {
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Creater { get; set; }
        public string Updater { get; set; }

    }
}
