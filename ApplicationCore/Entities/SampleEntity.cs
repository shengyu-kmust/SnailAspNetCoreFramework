using ApplicationCore.Enum;

namespace ApplicationCore.Entity
{
    public class SampleEntity:BaseEntity
    {
        public string Name { get; set; }
        public EGender Gender { get; set; }
    }
}
