using Web.Services;
using System;
using Xunit;
using DAL.Entity;
using CommonAbstract;
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
                var query = new StudentQueryDto();

                var result = cRUDService.Query(query, a => new StudentTestResult
                {
                    Id = a.Id,
                    Name = a.Name,
                    Gender = a.Gender,
                    CardId = a.Card.Id,
                    CardNo = a.Card.CardNo,
                    TeamId = a.Team.Id,
                    TeamName = a.Team.Name
                });
                //var result = cRUDService.Query(query, a => a);
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
