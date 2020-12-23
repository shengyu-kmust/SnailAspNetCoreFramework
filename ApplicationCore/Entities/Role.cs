using Snail.Core.Entity;
using Snail.Core.Permission;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    [Table("Role")]
    public class Role : DefaultBaseEntity, IRole
    {
        public string Name { get; set; }

        public string GetKey()
        {
            return this.Id;
        }

        public string GetName()
        {
            return this.Name;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }
        public void SetKey(string key)
        {
            this.Id = key;
        }
    }
}
