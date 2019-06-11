# SnailAspNetCoreFramework
asp.net core快速开发框架
基本的权限控制功能，要思考是否该用DDD和CQRS
要实现的功能
DTO:QueryModel,ViewModel?
1、DDD
2、CQRS
## 项目整体结构设计
采用DDD，但要根据项目的大小应用DDD的不同技术，总体是分如下几个项目
* CommonAbstract：抽象接口层
* DAL层：数据访问层，依赖于上层的接口，为上层（如服务层及应用层）提供数据服务。同时实现IDAL的通用数据访问接口。
* Application层：应用逻辑层，DDD模式，包含所有业务逻辑，不依赖于其它层
* Web层：负责接口参数的输入及输出的所有相关处理（如参数校验，输出格式预定等）、Application/DAL层的调用。

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
* ICache接口，实现redis,memcached，memoryCache等，可考虑用easycaching。经对easycaching的分析，决定不采用。
* 缓存用AOP，避免每个方法里写缓存逻辑
* 缓存的更新用事件驱动，引入第三方组件
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
* 用cap或是用mediator，暂时用mediator，因为cap为分布式，在中小项目上是用不着的
### 日志处理 **--已实现**
* 用nlog
### command bus **--已实现**
* 用Mediatr 
### DAL层
* 负责所有和数据相关的操作
* 封装通用的数据访问接口及实现，如用EF实现如下几个功能
* 用面向接口编程的思想，为以后DAL层采用EF外的技术做扩展
* 采用Repository模式
* 单表的查询，分页
* 多表的简单关联查询，分页
* 简单的CRUD
#### EF的使用
* EF执行sql语言的日志**--已实现**
* 对于枚举类型，用EF的ValueConverter技术自动转换，约定存储在数据库里的为string类型而不是Int类型，方便理解**--已实现**
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