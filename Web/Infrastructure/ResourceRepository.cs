using ApplicationCore.Entity;

namespace Web.Infrastructure
{
    public class ResourceRepository:BaseRepository<Resource>
    {

        public ResourceRepository(DatabaseContext db):base(db)
        {
        }

        
    }
}
