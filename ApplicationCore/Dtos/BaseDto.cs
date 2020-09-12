using Snail.Core.Dto;
using Snail.Core.Entity;
using System;

namespace ApplicationCore.Dtos
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
