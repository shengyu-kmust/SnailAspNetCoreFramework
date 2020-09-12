using Snail.Core.Entity;

namespace ApplicationCore.Entity
{
    public class UserOrg: DefaultBaseEntity
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }
    }
}
