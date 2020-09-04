#  Swagger是什么，为什么要用它，它解决了开发中的什么痛点问题？
* 在前后端分离的开发模式中，后端如何告知前端所有的接口地址，接口的输入输出约定，即后端接口文档的管理？
* 后端接口改变时，如何快速反馈给前端？
* 前端在代码开发前，是否能方便的测试和调用后端接口？
* 是否能根据接口定义，自动生成前端js或是后端语言的接口调用库，避免手动繁琐写代码？（此功能本文不做详细介绍）
**swagger即是解决如上问题的利器**

# 引入Swagger和简单的使用
* 参考https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-3.1&tabs=visual-studio
* Swagger其实是一种API文档规范，目前有很多实现此规范的框架或是工具，我们用NSwagger
* 从Nuget里引入包NSwag.AspNetCore

## 简单使用步骤
1.注册swagger
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerDocument();
}
```
2.将swagger加入到middlewares
```
public void Configure(IApplicationBuilder app)
{
    app.UseStaticFiles();
    // Register the Swagger generator and the Swagger UI middlewares
    app.UseOpenApi();
    app.UseSwaggerUi3();
    app.UseMvc();
}
```
3.访问swagger
* 从如下地址查看swagger的ui界面：http://localhost:<port>/swagger 
* 从如下地址查看swagger的接口描述文件：http://localhost:<port>/swagger/v1/swagger.json

# 其它功能

## 如何在swagger接口里显示接口的注释信息
在.csproj的PropertyGroup节点下加入如下节点即可
```
  <PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
```
> 这其实是dotnet的功能，当项目配置GenerateDocumentationFile后，会将项目里的注释生成xml，而swagger

# 如何对swagger进行权限验证

# 如何在生产和开发环境里停启用swagger