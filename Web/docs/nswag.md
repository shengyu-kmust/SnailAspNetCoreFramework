# swagger
**参考：https://github.com/RicoSuter/NSwag**
1、安装NSwag.AspNetCore
2、
public class Startup
{
    ...

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOpenApiDocument(); // add OpenAPI v3 document
//      services.AddSwaggerDocument(); // add Swagger v2 document
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        ...

        app.UseOpenApi(); // serve OpenAPI/Swagger documents
        app.UseSwaggerUi3(); // serve Swagger UI
	      app.UseReDoc(); // serve ReDoc UI
    }
}