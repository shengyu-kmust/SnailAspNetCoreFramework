using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotion.Linq.Parsing.Structure;
using DAL.Entity;

namespace DAL.Infrastructure
{
    public class OrganizationRepository:BaseRepository<Organization>
    {
        public OrganizationRepository(DatabaseContext db) : base(db)
        {
        }
    }
}
