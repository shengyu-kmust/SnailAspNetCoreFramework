using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entity;

namespace WebApp.Model
{
    public class ResourceModel
    {
        private Resource _resource;

        public ResourceModel(int id)
        {

        }

        public ResourceModel(string resourceKey)
        {

        }

        public static ResourceModel CreateResourceModel(int id)
        {
            return new ResourceModel(id);
        }

        public List<Role> AllPermissionRoles()
        {
            throw new NotImplementedException();
        }

    }
}
