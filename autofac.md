# autofac的使用
* container的生命周期和应用的生命周期一样，所以，**不要从container里直接resolve，而是从scope里resolve**

# 问题
## 如何在service层，或是web层获取autofac的IContainer对象，并手动resolve一个对象
在controller或是service里，用构造注入的方式注入ILifetimeScope,或是IComponentContext，再用前两者获取一个新的scope后，用scope的resove方法获得一个对象