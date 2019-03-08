using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entity;

namespace WebApp.Infrastructure
{
    public class PermissionRepository:BaseRepository<Permission>
    {
        public PermissionRepository(DatabaseContext db) : base(db)
        {
        }
    }
}
