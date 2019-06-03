using ApplicationCore.Entity;

namespace Web.Infrastructure
{
    public class OrganizationRepository:BaseRepository<Organization>
    {
        public OrganizationRepository(DatabaseContext db) : base(db)
        {
        }
    }
}
