using ApplicationCore.Enums;
using Snail.Core.Entity;

namespace ApplicationCore.Entities
{
    [EnableEntityCache]
    public class Config : DefaultBaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ExtraInfo { get; set; }
        public int? Rank { get; set; }
        public EConfigOperType OperType { get; set; }
    }
}
