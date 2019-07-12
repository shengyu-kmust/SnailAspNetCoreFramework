using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore;
using ApplicationCore.Abstract;
using AutoMapper;
using DAL;
using DALTest;
using DALTest.Entities;
using Microsoft.EntityFrameworkCore;
using Utility.Page;
using Xunit;

namespace ApplicationCoreTest
{
    public class ICRUDServiceTest
    {
        private StudentService _studentService;
        private TestDatabaseDbContext _db;
        public ICRUDServiceTest()
        {
            // 下面可以切换是用内存数据库和是sqlserver数据库
            _db = DatabaseDbContextHelper.GetInMemoryDbContext();//从helper里获取dbcontext
            //_db = DatabaseDbContextHelper.GetSqlServerDbContext();//从helper里获取dbcontext
            var repository = new EFRepository<Student>(_db);
            var mapper=new MapperConfiguration(a =>
            {
                a.CreateMap<Student, StudentResultDto>();
                a.CreateMap<StudentResultDto, Student>();
            }).CreateMapper();
            _studentService = new StudentService(mapper,_db, new EFRepository<Student>(_db));
        }

        [Fact]
        public void QueryPage()
        {
            try
            {
                var result = _studentService.QueryPage<StudentResultDto, StudentQueryDto>(new StudentQueryDto() { Name = "周晶" });
            }
            catch (Exception ex)
            {
            }
        }


        #region Service
        public class StudentService : CRUDService<Student, StudentResultDto, int>
        {
            private IMapper _mapper;
            private TestDatabaseDbContext _db;
            public StudentService(IMapper mapper,TestDatabaseDbContext db,IRepository<Student> repository):base(mapper,repository)
            {
                _mapper = mapper;
                _db = db;
                InitQuerySource();
            }
            public override void InitQuerySource()
            {
                _querySource = _mapper.ProjectTo<StudentResultDto>(_db.Students);
            }
        }
        #endregion

        #region Dto
        public class StudentResultDto:Student
        {
            public string TeamName { get; set; }
            public string CardNo { get; set; }
        }
        public class StudentQueryDto : IPagination, IPredicateConvert<StudentQueryDto,StudentResultDto>
        {
            public int PageSize { get; set; }
            public int PageIndex { get; set; }
            public string Name { get; set; }

            public Expression<Func<StudentResultDto, bool>> GetExpression()
            {
                return a => a.Name == "周晶" && 1==1;
            }
        }

        public class StudentSaveDto
        {

        }
        #endregion
    }

}
