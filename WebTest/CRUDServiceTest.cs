using ApplicationCore.Enum;
using System;
using Web.Services;
using Xunit;

namespace WebTest
{
    public class CRUDServiceTest : RepositoryBaseTest<Student>
    {
        public CRUDServiceTest()
        {
        }

        [Fact]
        public void Test1()
        {
           
        }
    }

    public class StudentTestResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public EGender? Gender { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int CardId { get; set; }
        public string CardNo { get; set; }
    }
  
}
