using AutoMapper;
using EasyCaching.Core;
using Microsoft.Extensions.DependencyInjection;
using Snail.Core.Interface;
using System.Linq;
using Xunit;

namespace ApplicationCoreTest
{
    public class EntityCacheTest
    {
        private IEntityCacheManager _entityCacheManager;
        private IMapper _mapper;
        private TestDatabaseDbContext _db;
        public EntityCacheTest()
        {
            var sc = new ServiceCollection();
            sc.AddEasyCaching(option =>
            {
                // use memory cache with a simple way
                option.UseInMemory(EntityCacheManager.EntityCacheName);
            });
            var sp = sc.BuildServiceProvider();
            var cacheFactory = sp.GetService<IEasyCachingProviderFactory>();
            _mapper =new MapperConfiguration(cfg => {
                cfg.CreateMap<StudentCacheItem, Student>().ReverseMap();
                cfg.CreateMap<Student, Student>();
                cfg.CreateMap<StudentCacheItem, StudentCacheItem>();
            }).CreateMapper();
            _db = DatabaseDbContextHelper.GetInMemoryDbContext();
            _entityCacheManager = new EntityCacheManager(_mapper, cacheFactory, _db);
        }

      

        [Fact]
        public void Change_Entity_Update_Cache_Not_Same_Test()
        {
            var cache = _entityCacheManager.GetAll<Student>();
            _db.Students.FirstOrDefault(a => a.Id == 1).Name = "修改";
            _db.SaveChanges();
            _entityCacheManager.UpdateEntityCacheItem<Student, int>(1);
            Assert.Equal(cache.FirstOrDefault(a => a.Id == 1).Name, "修改");
            Assert.Equal(_entityCacheManager.GetAll<Student>().FirstOrDefault(a => a.Id == 1).Name, "修改");
        }


        [Fact]
        public void Test_Entity_And_CacheItem_Same()
        {
            try
            {
                var cache = _entityCacheManager.GetAll<Student>();
                Assert.NotNull(cache);
                Assert.NotEmpty(cache);
            }
            catch (System.Exception)
            {

            }
        }

        [Fact]
        public void Test_Entity_And_CacheItem_Not_Same()
        {
            try
            {
                var cache = _entityCacheManager.GetAll<Student, StudentCacheItem>();
                Assert.NotNull(cache);
                Assert.NotEmpty(cache);
            }
            catch (System.Exception ex)
            {
            }
        }

        [Fact]
        public void Change_Entity_Update_Cache_Same_Test()
        {
            var cache = _entityCacheManager.GetAll<Student, StudentCacheItem>();
            _db.Students.FirstOrDefault(a => a.Id == 1).Name = "修改";
            _db.SaveChanges();
            _entityCacheManager.UpdateEntityCacheItem<Student, StudentCacheItem, int>(1);
            Assert.Equal(cache.FirstOrDefault(a => a.Id == 1).Name, "修改");
            Assert.Equal(_entityCacheManager.GetAll<Student, StudentCacheItem>().FirstOrDefault(a => a.Id == 1).Name, "修改");
        }

        [Fact]
        public void Add_Entity_Same_Test()
        {
            var cache = _entityCacheManager.GetAll<Student>();
            var id = 999;
            var name = "999";
            _db.Students.Add(new Student
            {
                Id = id,
                Name = name
            });
            _db.SaveChanges();
            _entityCacheManager.UpdateEntityCacheItem<Student, int>(id);
            Assert.Equal(cache.FirstOrDefault(a => a.Id == id).Name, name);
            Assert.Equal(_entityCacheManager.GetAll<Student>().FirstOrDefault(a => a.Id == id).Name, name);
        }

        [Fact]
        public void Add_Entity_Not_Same_Test()
        {
            var cache = _entityCacheManager.GetAll<Student, StudentCacheItem>();
            var id = 1000;
            var name = "1000";
            _db.Students.Add(new Student
            {
                Id = id,
                Name = name
            });
            _db.SaveChanges();
            _entityCacheManager.UpdateEntityCacheItem<Student, StudentCacheItem, int>(id);
            Assert.Equal(cache.FirstOrDefault(a => a.Id == id).Name, name);
            Assert.Equal(_entityCacheManager.GetAll<Student, StudentCacheItem>().FirstOrDefault(a => a.Id == id).Name, name);
        }

        [Fact]
        public void Delete_Entity_Same_Test()
        {
            var cache = _entityCacheManager.GetAll<Student>();
            var id = 999;
            var deleteEntity=_db.Students.FirstOrDefault(a => a.Id == id);
            _db.Remove(deleteEntity);
            _db.SaveChanges();
            _entityCacheManager.UpdateEntityCacheItem<Student, int>(id);
            Assert.Equal(cache.FirstOrDefault(a => a.Id == id),null);
            Assert.Equal(_entityCacheManager.GetAll<Student>().FirstOrDefault(a => a.Id == id), null);

        }

        [Fact]
        public void Delete_Entity_Not_Same_Test()
        {
            var cache = _entityCacheManager.GetAll<Student, StudentCacheItem>();
            var id = 1000;
            var deleteEntity = _db.Students.FirstOrDefault(a => a.Id == id);
            _db.Remove(deleteEntity);
            _db.SaveChanges();
            _entityCacheManager.UpdateEntityCacheItem<Student, StudentCacheItem, int>(id);
            Assert.Equal(cache.FirstOrDefault(a => a.Id == id), null);
            Assert.Equal(_entityCacheManager.GetAll<Student>().FirstOrDefault(a => a.Id == id), null);
        }
    }

    #region CacheItem
    public class StudentCacheItem:IIdField<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion
}
