using Snail.Core.Entity;

namespace ApplicationCore.Entity
{
    public class Org : DefaultBaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
    }
}
