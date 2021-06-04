# 项目介绍

* 本项目定位为基于asp.net core的中小企业快速开发框架，包含完整的后端和前端的代码。
* 目前正在开发v2.0版本，v1.0版本可在分支里切换后直接运行，教程可参考我的系列[博客](https://www.cnblogs.com/shengyu-kmust/p/13397738.html)，或是项目内的docs
* 后端技术栈：asp.net core 3.1,cap,swag,ef core code first,autofac,automapper,nlog,aop
* 前端技术栈：vue-element-admin,crud快速开发模块组件,mock
* 实现的功能：基于角色的权限控制，项目分层约定，代码自动生成，日志记录，性能监控，aop等

# 如何运行项目
```
git clone git@github.com:shengyu-kmust/SnailAspNetCoreFramework.git
git clone git@github.com:shengyu-kmust/Snail.git
cd SnailAspNetCoreFramework
dotnet build
```

# 项目演示
**如下是基于v1.0版本的演示图**
* 1、下载和运行项目
![下载和运行项目](https://img2020.cnblogs.com/blog/677927/202007/677927-20200729211834784-1170057261.gif)
* 2、根据配置文件自动生成项目
![根据配置文件自动生成项目](https://img2020.cnblogs.com/blog/677927/202007/677927-20200729211954491-179563083.gif)
* 3、运行项目
![运行项目](https://img2020.cnblogs.com/blog/677927/202007/677927-20200729212037499-1649891473.gif)

# 项目目录介绍
--ApplicationCore // 核心抽象层，采用clear architecture模式，不依赖于其它层，其它三个项目都依赖此类库，负责接口、常量、枚举、dto、实体等公共定义  
----Const // 常量定义，如配置常量，事件名常量  
----Dtos // 所有的dto  
----Entities // 所有实体  
----Enums // 枚举  
----IServices // Service层的抽象定义  
----Utilities // 帮助类  

--Infrastructure // 基础设施层，为上层（如服务层及应用层）提供数据服务。  
----Data/config // 数据库entityframework fluent api配置  
----EFValueConverter // entityframework的数据库类型和clr类型的转换，如枚举转换  
----Migrations //为entityframework code first的migrate生成目录  



--Service // 服务层，用于实现ApplicationCore里的服务接口，为应用的逻辑实现的主层  
----Cache // 缓存实现，后面会移除并抽离到Snail项目  
----Interceptor // 默认实现的拦截器  
----BaseService.cs // 各service的基类，包含了各service的常用方法，如CRUD  
----InitDatabaseService.cs // 负责数据库的数据初始化  
----InterceptorService.cs // 拦截器基类  


--Web // 负责接口参数的输入及输出的所有相关处理（如参数校验，输出格式预定等）  
----AutoFacModule // autofac的注入配置  
----AutoMapperProfiles // automapper的配置  
----ClientApp // 前端项目  
------build // 前端编译生成的输出目录  
------mock // 前端mock  
------src // 前端核心代码  
----CodeGenerater // 代码生成的t4模块和相关逻辑  
----ConfigureServicesExtenssions // serviceProvider的注入扩展，以避免写在startup文件里  
----Controllers // 控制器  
----docs // 文档集  
----Dto // 只会在web层里用到的dto类  
----Filter // 过滤器  
----Hubs // signalr  
----Permission // 权限的默认实现  
----staticFile // 用于存储上传的文件  

## 项目整体结构设计
采用DDD，但要根据项目的大小应用DDD的不同技术，总体是分如下几个项目
* Service：服务层，用于实现ApplicationCore里的服务接口，为应用的逻辑实现的主层
* Infrastructure层：基础层，此层在不同的项目里进行复用，依赖于ApplicationCore层的接口约定，为上层（如服务层及应用层）提供数据服务。同时实现IDAL的通用数据访问接口。
* ApplicationCore层：应用逻辑层，DDD模式，包含所有业务逻辑，不依赖于其它层
* Web层：负责接口参数的输入及输出的所有相关处理（如参数校验，输出格式预定等）、Application/DAL层的调用。
### 通用数据权限及验证
### WEB层
* 负责输入输出参数处理、异常拦截、web缓存
* 对于查询统计类型的数据操作，可直接调用DAL层
* 开发和测试环境的配置可分开
#### 参数校验 **--已实现**
* 用微软的接口 **--已实现**
#### 输出约定 **--已实现**
* HTTP Status Code遵照状态码的约定，20x为成功（接口已经调用成功），40x为请求错误（业务异常、未授权等），50x为服务器错误（服务器的异常） **--已实现**
* 返回结果内容以实际前端要的数据格式为主，为json对象；如果是错误提示，返回的是错误内容。 **--已实现**
#### 异常处理  **--已实现**
* 异常处理用微软的技术，参考https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/error-handling?view=aspnetcore-2.2
* 所有的异常都不会捕获
* Business异常向外提示，服务器异常做拦截并向外提示，提示输出格式遵照输出约定
#### swagger **--已实现**
* 用NSwagger 
### ApplicationCore层
* 此项目不要依赖任何的其它项目，用干净体系结构，参考：https://docs.microsoft.com/zh-cn/dotnet/standard/modern-web-apps-azure-architecture/common-web-application-architectures

### 缓存
* ICache接口，实现redis,memcached，memoryCache等，可考虑用easycaching。
* 缓存用AOP，避免每个方法里写缓存逻辑。easycaching+autofac
* 缓存的更新用事件驱动，引入第三方组件。easycaching+cap
### 领域驱动
领域驱动，如果依赖其它层，依赖于对应层的接口，而不是实现
### AOP **--已实现**
* 日志、缓存的AOP切入，考虑用autofac
### IOC **--已实现**
* 用autofac
### 内存表 **--已实现**
* 将不常变动的、数据量比较小的表存储在内存里（不是缓存，是对象），并用事件驱动的思想同步最新的数据
### 事件
* 发布和订阅
* 用cap，可在分布式和单应用间切换
### 日志处理 **--已实现**
* 用nlog
### command bus **--已实现**
* 用Mediatr 
### DAL层
* 负责所有和数据相关的操作
* 封装通用的数据访问接口及实现，如用EF实现如下几个功能
* 用面向接口编程的思想，为以后DAL层采用EF外的技术做扩展
* 采用Repository模式，其实EF本身就是一个Repository模式，但为了统一在数据库访问操作这一层进行拦截并应用自己的逻辑，再用自己的Repository对EF进行一个封装
* 单表的查询，分页
* 多表的简单关联查询，分页
* 简单的CRUD
### automapper
* 引用automapper做对象的赋值转换
* 实现基于expression的objectmapper类，用于简单的对象赋值，弥补automapper的配置复杂、用反射的性能低下的缺点
#### EF的使用
* EF执行sql语言的日志**--已实现**
* 对于枚举类型，用EF的ValueConverter技术自动转换，约定存储在数据库里的为string类型而不是Int类型，方便理解**--已实现**

### 实时通讯
* 用signalr

## 技术选型
###
### 事务的处理
用repository和unit of work模式，但repository模式用EF自带的
### 数据库处理方式
* 用EF做数据处理，不考虑其它的方式。


### 前端界面
* 放入到web项目里，用spa的方式

## 需要做的
* 单元测试的注入
* service层应该不在web层里，而是applicationcore

### 多语言



## 各技术细节
### CAP
* CAP的原始实现是对autofac的支持不是太好，本项目里编写SnailCapConsumerServiceSelector来做处理
* CAP原生对Controller和非Controller的处理不一样，Controller只要在方法上加上CapSubscribe特性就行，而埋Controller要在类上实现ICapSubscribe接口，并在方法上加上CapSubscribe特性

## 使用方法
1、git clone此项目
2、配置codeGenerate.json
3、运行此项目，并在swagger里登录后，运行codeGenerate接口
4、运行add-migration
5、启动项目即可


# 教程博文
[SnailAspNetCoreFramework框架系列博客](https://www.cnblogs.com/shengyu-kmust/p/13397738.html)  
[1.框架内各项目及目录的介绍和总设计思路——SnailAspNetCoreFramework快速开发框架](https://www.cnblogs.com/shengyu-kmust/p/13453773.html)  
[2.接口输入校验、输出格式、及异常处理——SnailAspNetCoreFramework快速开发框架之后端设计](https://www.cnblogs.com/shengyu-kmust/p/13453791.html)  
[3.通用权限设计——SnailAspNetCoreFramework快速开发框架之后端设计](https://www.cnblogs.com/shengyu-kmust/p/13453799.html)    
4.如何提供给前端良好的接口文档（Swagger）——SnailAspNetCoreFramework快速开发框架之后端设计  
5.各场景下的缓存使用——SnailAspNetCoreFramework快速开发框架之后端设计  
6.基于castle的AOP设计和常用缓存、性能、日志拦截器实现——SnailAspNetCoreFramework快速开发框架之后端设计  
7.依赖注入介绍之autofac——SnailAspNetCoreFramework快速开发框架之后端设计  
8.为什么用Eventbus，怎么用——SnailAspNetCoreFramework快速开发框架之后端设计  
9.日志组件之Nlog介绍——SnailAspNetCoreFramework快速开发框架之后端设计  
10.Mediatr介绍——SnailAspNetCoreFramework快速开发框架之后端设计  
11.controller、service、dal层的通用CRUD设计——SnailAspNetCoreFramework快速开发框架之后端设计  
12.对象映射之利器automapper——SnailAspNetCoreFramework快速开发框架之后端设计  
13.如何监控ef生成的sql语句——SnailAspNetCoreFramework快速开发框架之后端设计  
14..net core的几种部署方式介绍——SnailAspNetCoreFramework快速开发框架之后端设计  
15.实时通讯之signalr——SnailAspNetCoreFramework快速开发框架之后端设计  
16.如何用ef code first进行项目的数据库版本持续迭代——SnailAspNetCoreFramework快速开发框架之后端设计  
17.代码自动生成功能介绍——SnailAspNetCoreFramework快速开发框架之后端设计  
18.如何监控项目的各个功能是否正常(HealthCheck)——SnailAspNetCoreFramework快速开发框架之后端设计  
19.定时任务之hangfire介绍——SnailAspNetCoreFramework快速开发框架之后端设计  
20.前端总体介绍——SnailAspNetCoreFramework快速开发框架之前端设计  
21.再也不用跪求后端接口了（MOCK的使用）——SnailAspNetCoreFramework快速开发框架之前端设计  
22.前端的权限控制——SnailAspNetCoreFramework快速开发框架之前端设计  
23.如何避免重复写前端的CRUD代码（前端各种SnailXXX组件介绍和使用）——SnailAspNetCoreFramework快速开发框架之前端设计  
24.请求代理介绍——SnailAspNetCoreFramework快速开发框架之前端设计  
 























