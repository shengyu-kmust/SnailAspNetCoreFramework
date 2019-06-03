using ApplicationCore.Entity;

namespace Web.Infrastructure
{
    public class PermissionRepository:BaseRepository<Permission>
    {
        public PermissionRepository(DatabaseContext db) : base(db)
        {
        }
    }
}
