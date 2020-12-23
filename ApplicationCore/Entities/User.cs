using Snail.Core.Entity;
using Snail.Core.Enum;
using Snail.Core.Permission;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    [Table("User")]
    public partial class User : DefaultBaseEntity, IUser
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

        public void SetAccount(string account)
        {
            this.Account = account;
        }

        public void SetKey(string key)
        {
            this.Id = key;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetPassword(string pwd)
        {
            this.Pwd = pwd;
        }
    }
}
