## automapper
1、引入 AutoMapper.Extensions.Microsoft.DependencyInjection
2、di
public void ConfigureServices(IServiceCollection services)加入如下
services.AddAutoMapper(Assembly.GetExecutingAssembly());
3、创建profile类，并配置
public class EntityDtoProfile : Profile
    {
        public EntityDtoProfile()
        {
            //配置automapper
        }
    }