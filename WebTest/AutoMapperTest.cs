using AutoMapper;
using Xunit;

namespace WebTest
{
    public class AutoMapperTest
    {
        private IMapper _mapper;
        public AutoMapperTest()
        {
            _mapper=new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AutoMapperA, AutoMapperA1>();
                cfg.CreateMap<AutoMapperA, AutoMapperA1>();
                cfg.CreateMap<AutoMapperA, AutoMapperA1>();
                cfg.CreateMap<AutoMapperA, AutoMapperA1>();
            }).CreateMapper();
        }


        [Fact]
        public void Test()
        {
            _mapper.Map(new AutoMapperA { }, new AutoMapperA1 { });
        }
    }
    
    public class AutoMapperA
    {
        public int Id { get; set; }
    }
    public class AutoMapperA1
    {
        public int Id { get; set; }
    }
    public class AutoMapperA2
    {
        public int Id { get; set; }
    }
}
