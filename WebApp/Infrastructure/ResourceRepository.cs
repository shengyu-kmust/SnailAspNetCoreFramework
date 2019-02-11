using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entity;

namespace WebApp.Infrastructure
{
    public class ResourceRepository:BaseRepository<Resource>
    {

        public ResourceRepository(DatabaseContext db):base(db)
        {
        }

        
    }
}
