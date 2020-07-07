## 拦截器用castle（autofac内部用）
### 拦截的配置和使用
* serviceModule里默认配置以class的方法进行拦截
* 将要拦截的方法改成virtual，并在class里写好InterceptAttribute
* 多个拦截的先后，A,B(A写在上面，B写在下面)，执行顺序为B前->A前->A后->B后，类似于asp.net core的管道
### 缓存的拦截
* 对于controller的缓存，用responseCache
* 对于service的缓存，用cacheInterceptor
### 日志和性能拦截
* 用LogInterceptor，可在.json文件里配置LogInterceptorOption