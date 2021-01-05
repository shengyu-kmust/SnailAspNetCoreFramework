* 基础层的责任是提供数据访问、日志、邮件、支付等底层功能，以及对applicationCore层的部分和底层有关的service的实现（如直接和数据库相关在查询报表）

# 如何用migration
* 在infrastructure项目里加入Microsoft.EntityFrameworkCore.Tools包，将PM里的启动项目设置成infrastructure，
* 在DesignTimeDbContextFactory里定义migrate的数据库信息
* 用IEntityTypeConfiguration方法定义每一个entity的配置
* 在pm里，通过在项目infrastructure里运行add-migration和update-database等命令来同步数据库结构
* 如果要重新生成，删除生成的migrations文件即可，再重新运行命令

# 用dotnet-ef cli管理migration
* 参考:https://docs.microsoft.com/zh-cn/ef/core/cli/dotnet
* 安装dotnet-ef cli，用命令dotnet tool install --global dotnet-ef
* migration命令，dotnet ef migrations add init -p Infrastructure -s Web // -p为目标项目，-s为启动项目