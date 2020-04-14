using Snail.Core.Enum;

namespace ApplicationCore.Entity
{
    [EnableEntityCache]
    public class SampleEntity:BaseEntity
    {
        public string Name { get; set; }
        public EGender Gender { get; set; }
        public int Age { get; set; }
    }
}
