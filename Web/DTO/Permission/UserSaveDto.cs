using Snail.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.DTO.Permission
{
    public class UserSaveDto
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public EGender Gender { get; set; }
    }
}
