using DAL;
using DALTest.Entities;
using System.Linq;
using Xunit;

namespace DALTest
{
    public class EFRepositoryTest
    {
        [Fact]
        public void Test()
        {
            var db = TestDatabaseDbContext.GetDbContext();
            var temp = db.Students.ToList();
            var repository= new EFRepository<Student>(db);
            var result=repository.Query(null, null, null, a =>new 
               {
                   a.Id,
                   a.Name,
                   TeamName=a.Team.Name,
                   a.identityCard.CardNo,
                   a.BankCards
               });

        }
    }
}
