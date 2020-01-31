using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Web
{
    /// <summary>
    /// 由于CAP默认的ConsumerServiceSelector不能和autofac进行很好的兼容，这里重新实现。用法
    /// </summary>
    /// <remarks>
    /// 除方法FindConsumersFromInterfaceTypes，其它方法和ConsumerServiceSelector里的是一样的
    /// </remarks>
    public class SnailCapConsumerServiceSelector : IConsumerServiceSelector
    {
        private readonly CapOptions _capOptions;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// since this class be designed as a Singleton service,the following two list must be thread safe!
        /// </summary>
        private readonly ConcurrentDictionary<string, List<RegexExecuteDescriptor<ConsumerExecutorDescriptor>>> _asteriskList;
        private readonly ConcurrentDictionary<string, List<RegexExecuteDescriptor<ConsumerExecutorDescriptor>>> _poundList;

        /// <summary>
        /// Creates a new <see cref="ConsumerServiceSelector" />.
        /// </summary>
        public SnailCapConsumerServiceSelector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _capOptions = serviceProvider.GetService<IOptions<CapOptions>>().Value;

            _asteriskList = new ConcurrentDictionary<string, List<RegexExecuteDescriptor<ConsumerExecutorDescriptor>>>();
            _poundList = new ConcurrentDictionary<string, List<RegexExecuteDescriptor<ConsumerExecutorDescriptor>>>();
        }

        public IReadOnlyList<ConsumerExecutorDescriptor> SelectCandidates()
        {
            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

            executorDescriptorList.AddRange(FindConsumersFromInterfaceTypes(_serviceProvider));

            executorDescriptorList.AddRange(FindConsumersFromControllerTypes());

            return executorDescriptorList;
        }

        public ConsumerExecutorDescriptor SelectBestCandidate(string key, IReadOnlyList<ConsumerExecutorDescriptor> executeDescriptor)
        {
            if (executeDescriptor.Count == 0)
            {
                return null;
            }

            var result = MatchUsingName(key, executeDescriptor);
            if (result != null)
            {
                return result;
            }

            //[*] match with regex, i.e.  foo.*.abc
            result = MatchAsteriskUsingRegex(key, executeDescriptor);
            if (result != null)
            {
                return result;
            }

            //[#] match regex, i.e. foo.#
            result = MatchPoundUsingRegex(key, executeDescriptor);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <remarks>
        /// 此方法是获取所有事件的名和事件对应的处理方法的描述信息，和cap的默认实现不一样
        /// </remarks>
        protected virtual IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromInterfaceTypes(
            IServiceProvider provider)
        {
            // 原cap的实现如下，是从ServiceCollection里获取所有的事件处理方法，这样会造成在autofac里注册的类不能处理事件
            //var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

            //var capSubscribeTypeInfo = typeof(ICapSubscribe).GetTypeInfo();

            //foreach (var service in ServiceCollectionExtensions.ServiceCollection.Where(o => o.ImplementationType != null && o.ServiceType != null))
            //{
            //    var typeInfo = service.ImplementationType.GetTypeInfo();
            //    if (!capSubscribeTypeInfo.IsAssignableFrom(typeInfo))
            //    {
            //        continue;
            //    }
            //    var serviceTypeInfo = service.ServiceType.GetTypeInfo();

            //    executorDescriptorList.AddRange(GetTopicAttributesDescription(typeInfo, serviceTypeInfo));
            //}

            //return executorDescriptorList;



            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

            using (var scoped = provider.CreateScope())
            {
                var scopedProvider = scoped.ServiceProvider;
                var consumerServices = scopedProvider.GetServices<ICapSubscribe>();
                foreach (var service in consumerServices)
                {
                    var typeInfo = service.GetType().GetTypeInfo();
                    if (!typeof(ICapSubscribe).GetTypeInfo().IsAssignableFrom(typeInfo))
                    {
                        continue;
                    }

                    executorDescriptorList.AddRange(GetTopicAttributesDescription(typeInfo));
                }

                return executorDescriptorList;
            }
        }

        /// <summary>
        /// 所有的controller类，不需要实现ICapSubscribe接口，只要有CapSubscribeAttribute就行，非controller类要实现ICapSubscribe接口
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromControllerTypes()
        {
            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

            var types = Assembly.GetEntryAssembly().ExportedTypes;
            foreach (var type in types)
            {
                var typeInfo = type.GetTypeInfo();
                if (Helper.IsController(typeInfo))
                {
                    executorDescriptorList.AddRange(GetTopicAttributesDescription(typeInfo));
                }
            }

            return executorDescriptorList;
        }

        protected IEnumerable<ConsumerExecutorDescriptor> GetTopicAttributesDescription(TypeInfo typeInfo, TypeInfo serviceTypeInfo = null)
        {
            foreach (var method in typeInfo.DeclaredMethods)
            {
                var topicAttr = method.GetCustomAttributes<TopicAttribute>(true);
                var topicAttributes = topicAttr as IList<TopicAttribute> ?? topicAttr.ToList();

                if (!topicAttributes.Any())
                {
                    continue;
                }

                foreach (var attr in topicAttributes)
                {
                    SetSubscribeAttribute(attr);

                    var parameters = method.GetParameters()
                        .Select(parameter => new ParameterDescriptor
                        {
                            Name = parameter.Name,
                            ParameterType = parameter.ParameterType,
                            IsFromCap = parameter.GetCustomAttributes(typeof(FromCapAttribute)).Any()
                        }).ToList();

                    yield return InitDescriptor(attr, method, typeInfo, serviceTypeInfo, parameters);
                }
            }
        }

        protected virtual void SetSubscribeAttribute(TopicAttribute attribute)
        {
            if (attribute.Group == null)
            {
                attribute.Group = _capOptions.DefaultGroup + "." + _capOptions.Version;
            }
            else
            {
                attribute.Group = attribute.Group + "." + _capOptions.Version;
            }
        }

        private static ConsumerExecutorDescriptor InitDescriptor(
            TopicAttribute attr,
            MethodInfo methodInfo,
            TypeInfo implType,
            TypeInfo serviceTypeInfo,
            IList<ParameterDescriptor> parameters)
        {
            var descriptor = new ConsumerExecutorDescriptor
            {
                Attribute = attr,
                MethodInfo = methodInfo,
                ImplTypeInfo = implType,
                ServiceTypeInfo = serviceTypeInfo,
                Parameters = parameters
            };

            return descriptor;
        }

        private ConsumerExecutorDescriptor MatchUsingName(string key, IReadOnlyList<ConsumerExecutorDescriptor> executeDescriptor)
        {
            return executeDescriptor.FirstOrDefault(x => x.Attribute.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        private ConsumerExecutorDescriptor MatchAsteriskUsingRegex(string key, IReadOnlyList<ConsumerExecutorDescriptor> executeDescriptor)
        {
            var group = executeDescriptor.First().Attribute.Group;
            if (!_asteriskList.TryGetValue(group, out var tmpList))
            {
                tmpList = executeDescriptor.Where(x => x.Attribute.Name.IndexOf('*') >= 0)
                    .Select(x => new RegexExecuteDescriptor<ConsumerExecutorDescriptor>
                    {
                        Name = ("^" + x.Attribute.Name + "$").Replace("*", "[0-9_a-zA-Z]+").Replace(".", "\\."),
                        Descriptor = x
                    }).ToList();
                _asteriskList.TryAdd(group, tmpList);
            }

            foreach (var red in tmpList)
            {
                if (Regex.IsMatch(key, red.Name, RegexOptions.Singleline))
                {
                    return red.Descriptor;
                }
            }

            return null;
        }

        private ConsumerExecutorDescriptor MatchPoundUsingRegex(string key, IReadOnlyList<ConsumerExecutorDescriptor> executeDescriptor)
        {
            var group = executeDescriptor.First().Attribute.Group;
            if (!_poundList.TryGetValue(group, out var tmpList))
            {
                tmpList = executeDescriptor
                    .Where(x => x.Attribute.Name.IndexOf('#') >= 0)
                    .Select(x => new RegexExecuteDescriptor<ConsumerExecutorDescriptor>
                    {
                        Name = ("^" + x.Attribute.Name.Replace(".", "\\.") + "$").Replace("#", "[0-9_a-zA-Z\\.]+"),
                        Descriptor = x
                    }).ToList();
                _poundList.TryAdd(group, tmpList);
            }

            foreach (var red in tmpList)
            {
                if (Regex.IsMatch(key, red.Name, RegexOptions.Singleline))
                {
                    return red.Descriptor;
                }
            }

            return null;
        }


        private class RegexExecuteDescriptor<T>
        {
            public string Name { get; set; }

            public T Descriptor { get; set; }
        }
    }
}
