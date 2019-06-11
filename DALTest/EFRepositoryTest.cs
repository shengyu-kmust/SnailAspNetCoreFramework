using ApplicationCore.Abstract;
using DAL;
using DALTest.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DALTest
{
    public class EFRepositoryTest
    {
        private IRepository<Student> _repository;
        private TestDatabaseDbContext _db;
        public EFRepositoryTest()
        {
            _db = DatabaseDbContextHelper.GetInMemoryDbContext();
            _repository = new EFRepository<Student>(_db);
        }

        [Fact]
        public void Update_Test()
        {
            var entity = _repository.FirstOrDefault();
            var updateEntity = new Student
            {
                Id = entity.Id,
                Name = "修改",
                BirthDay = new DateTime(2000, 12, 1)
            };
            _repository.Update(updateEntity, new List<string> { "BirthDay" });
            var updated = _repository.FirstOrDefault(a => a.Id == entity.Id);
            Assert.Equal(entity.Name, updated.Name);//名字应该改不了
            Assert.Equal(new DateTime(2000, 12, 1), updated.BirthDay);//只能改生日
        }

        [Fact]
        public void db_update_all_Test()
        {
            var entity=_db.Students.AsNoTracking().FirstOrDefault();
            _db.Students.Update(new Student
            {
                Id = entity.Id,
                Name = "修改",
                BirthDay = new DateTime(2000, 1, 3)
            });
            _db.SaveChanges();
            var updated = _db.Students.FirstOrDefault(a => a.Id == entity.Id);

        }

        [Fact]
        public void db_update_some_Test()
        {
            var entity = _db.Students.FirstOrDefault();
            //var entity = _db.Students.AsNoTracking().FirstOrDefault();
            var entry=_db.Entry(new Student
            {
                Id = entity.Id,
                Name = "不能修改",
                BirthDay = new DateTime(2000, 1, 3)
            });
            entry.Property("BirthDay").IsModified = true;
            _db.SaveChanges();
            var updated = _db.Students.FirstOrDefault(a => a.Id == entity.Id);

        }
    }
}
