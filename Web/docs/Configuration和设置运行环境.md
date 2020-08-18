# Configuration
    应用程序通常会从外部读取配置，这些配置可能来源于配置文件，或是命令行，或是系统的环境变量等等，asp.net core提供了一套配置处理类库处理此类问题，
即Microsoft.Extensions.Configuration。
* 所有的配置源在加载后，都以key-value的形式存储在字典里，key支持父子层级结构，层级结构默认以符号“:”隔开，如key=User:Name
* 默认的配置加载顺序为appsettings.json->appsettings.Enviroment.json->app secrets（当Development环境时）->环境变量->命令行，后加载的会覆盖前面的
* 本文以获取配置test下的name的值为示例，即：Configuration.GetValue<string>("test:name")
## 从环境变量设置Configuration
* 在cmd命令和Powershell命令下设置环境变量的方式是不同的
* 符号“:”只是对于window环境有效，对于linux环境，用“__”代替（双下划线），所以建议所有的:都用__代替，以兼容写法
### 在cmd里设置环境变量
set cmdNameXXX=cmdValue，注意cmdValue不用引号包裹
```
 set test__name=this is test.name value from cmd
```

### 在powershell里设置环境变量
$env:cmdNameXXX="cmd value"，注意cmdValue要用引号包裹
```
 $env:test__name="this is test.name from powershell"
```

## 从命令行设置Configuration
* 和环境变量不同，命令行设置时不需要将“:”替换为“__”，如果配置是有层级的，请用:
* 格式为：--key=value，如果value里有空格，请用引号将value包进来，如--test:name="this is test.name value"
```
dotnet run --test:name="this is test.name from cmdline"
dotnet run test:name=testNameValue
dotnet run --test:name=testNamveValue
```
## 从配置文件设置
示例json:
```
{
   "User": {
        "Name": "zhangsang",
        "Age": 32
    },
   "Pwd": "123456"
}
```
存储在字典为如下数据
key:"User:Name",value:"zhangsang"
key:"User.Age",value:32
key:Pwd,value:"123456"



# 设置运行环境
* 运行环境是从环境变量里获取，即可参考上面“从环境变量设置Configuration”这一章节
* asp.net core的内置运行环境为Development,Staging,Production，也可以自己定义
* 如果asp.net core 未使用默认设置（即使用ConfigureWebHostDefaults），会获取名为DOTNET_ENVIRONMENT的环境变量的值做为运行环境；否则从环境变量ASPNETCORE_ENVIRONMENT获取运行环境

## 如何设置运行环境

### 通过设置运行环境变量ASPNETCORE_ENVIRONMENT的值来设置运行环境（生产环境使用的方式）
* 在cmd命令里
```
set ASPNETCORE_ENVIRONMENT=Staging
dotnet run --no-launch-profile
```
* 在powershell里
```
$Env:ASPNETCORE_ENVIRONMENT = "Staging"
dotnet run --no-launch-profile
```
**需注意--no-launch-profile参数，因dotnet run默认会去当前目录下找Properties/launchSettings.json，并将文件里的配置做为环境变量，用--no-launch-profile命令排除此文件以避免此文件对环境变量的影响**

### 通过launchSetting.json设置环境环境（开发环境使用的方式）
launchSetting.json在项目的Properties目录下，一般会配置多种配置方案，供开发时使用，如下
```
{
  // 配置了IIS Express,WebAppDevelopment,WebAppProduction三种方案
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchUrl": "api/values",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "WebAppDevelopment": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger/index.html",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    },
    "WebAppProduction": {
      "commandName": "Project",
      "launchUrl": "swagger/index.html",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    }
  }
}
```
如何采用方案的命令示例如下
```
dotnet run --launch-profile WebAppProduction
dotnet run --launch-profile WebAppDevelopment
```


## 如何在代码里判断运行环境
env为IHostEnvironment，下面几种方法都可
```
 env.IsDevelopment();// 判断是否为开发环境
 env.IsProduction();// 判断是否为生产环境
 env.IsStaging();// 判断是否为测试环境
 env.IsEnvironment("XX");// 判断是否为XX环境
```

