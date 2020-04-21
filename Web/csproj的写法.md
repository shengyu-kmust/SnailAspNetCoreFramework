

## 项目属性PropertyGroup
* 属性是可用于配置生成的名称/值对，可以自己定义任何名字
* 可以用$(xxx) 进行引用定义好的属性
* 预留的属性有https://docs.microsoft.com/zh-cn/visualstudio/msbuild/msbuild-reserved-and-well-known-properties?view=vs-2019
## 项目项ItemGroup
* 项是对一个或多个文件的命名引用。 项包含元数据（如文件名、路径和版本号）。 Visual Studio 中的所有项目类型具有几个通用项。 在文件 Microsoft.Build.CommonTypes.xsd 中定义了这些项
* 参考（https://docs.microsoft.com/zh-cn/visualstudio/msbuild/common-msbuild-project-items?view=vs-2019）
* 项必需有Include/Exclude/Remove等属性
--Content：表示不会编译到项目中，但可能会嵌入到其中或随其一起发布的文件，属性CopyToOutputDirectory有Never、Always、PreserveNewest
  示例:<Content Include="ClientApp\dist\**\*" CopyToPublishDirectory="Always" CopyToOutputDirectory="Always" />，即在生成和publish时，都会复制ClientApp\dist里所有文件夹里的文件到目录


