using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class TestData
    {
        public static List<User> Users = new List<User>
        {
            new User {Id = "001", Name = "zhoujing",Role="admin", Email = "373087455@qq.com", Pwd = "123"},
            new User {Id = "002", Name = "majuan",Role = "customer",Email = "2098347@qq.com", Pwd = "123"}
        };
    }

    public class User
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Role { get; set; }
        public string Email { set; get; }
        public string Pwd { set; get; }
    }
}
