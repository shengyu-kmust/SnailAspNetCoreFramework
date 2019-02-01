using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entity;

namespace WebApp.Model
{
    public class UserModel
    {
        private User _user;
        private DatabaseContext _db;
        public UserModel(User user,DatabaseContext db)
        {
            _user = user;
            _db = db;
        }

        public UserModel(int userId)
        {
        }

        public bool HasPermission(Resource resource)
        {
            var myRoles = MyRoles();
            var resourceModel = ResourceModel.CreateResourceModel(resource.Id);
            var resourceRoles = resourceModel.AllPermissionRoles();
            return myRoles.Intersect(resourceRoles).Any();
        }

        public List<Role> MyRoles()
        {
            throw new NotImplementedException();
        }

    }
}
