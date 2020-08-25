using Snail.Core.Entity;
using Snail.Core.Enum;
using Snail.Core.Permission;

namespace ApplicationCore.Entity
{
    public partial class User: DefaultBaseEntity, IUser
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public EGender Gender { get; set; }

      

        public string GetAccount()
        {
            return Account;
        }

        public string GetKey()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetPassword()
        {
            return Pwd;
        }
    }
}
