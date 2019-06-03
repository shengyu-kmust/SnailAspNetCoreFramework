using Web.Services;
using System;
using Xunit;
using DAL.Entity;
using ApplicationCore.Abstract;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Web.DTO.Sample;
using ApplicationCore.Enum;

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
            try
            {
                CRUDService<Student> cRUDService = new CRUDService<Student>(Repository);
                var saveDto = new StudentSaveDto()
                {
                    Id = 1,
                    Gender=Gender.Female
                };

                cRUDService.Update(saveDto);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }
    }

    public class StudentTestResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int CardId { get; set; }
        public string CardNo { get; set; }
    }
  
}
