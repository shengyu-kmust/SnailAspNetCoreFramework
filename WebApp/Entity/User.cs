using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entity
{
    [Table("User")]
    public class User:BaseEntity
    {
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public int Gender { get; set; }
    }
}
