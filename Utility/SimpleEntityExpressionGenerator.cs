using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utility.Utilities
{
    /// <summary>
    /// 根据entity生成简单的表达式
    /// <remarks>
    /// 后期可加入dto和entity映射的缓存机制来提高效率
    /// </remarks>
    /// </summary>
    public static class SimpleEntityExpressionGenerator
    {
        public static void Test()
        {

        }

        /// <summary>
        /// 根据dto对象生成简单的T表的表达式树，此表达式树只有与关系
        /// </summary>
        /// <typeparam name="T">生成哪个表T，的表达式树</typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GenerateAndExpressionFromDto<T>(object dto,Expression<Func<T,bool>> extraExpression=null)
        {
            if (dto==null)
            {
                Expression<Func<T, bool>> expression = a => 1 == 1;
                return expression;
            }
            // 可空类型的IsGenericType=true
            var dtoProperties = dto.GetType().GetProperties().Where(a =>
                a.PropertyType.IsPublic && !a.PropertyType.IsAbstract &&
                !a.PropertyType.IsArray).ToList();
            var entityProperties = typeof(T).GetProperties().Where(a =>
                a.PropertyType.IsPublic && !a.PropertyType.IsAbstract &&
                !a.PropertyType.IsArray).ToList();
            var param = Expression.Parameter(typeof(T));
            var filterExpression = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
            foreach (var dtoPropertyInfo in dtoProperties)
            {
                var dtoPropertyValue = dtoPropertyInfo.GetValue(dto);

                if (CanGenerateExpressionFromDtoProperty(entityProperties, dtoPropertyInfo, dtoPropertyValue, param, out Expression binaryFilterExpression))
                {
                    filterExpression =
                        Expression.And(binaryFilterExpression, filterExpression);
                }
            }

            if (extraExpression!=null)
            {
              return  Expression.Lambda<Func<T, bool>>(filterExpression, param).And(extraExpression);
            }
            else
            {
                return Expression.Lambda<Func<T, bool>>(filterExpression, param);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityPropertyInfos"></param>
        /// <param name="dtoPropertyInfo"></param>
        /// <param name="dtoPropertyValue"></param>
        /// <param name="param"></param>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public static bool CanGenerateExpressionFromDtoProperty(List<PropertyInfo> entityPropertyInfos, PropertyInfo dtoPropertyInfo, object dtoPropertyValue, ParameterExpression param, out Expression filterExpression)
        {
            BinaryExpressionFilterType filterType;
            PropertyInfo entityMatchPropertyInfo;
            filterExpression = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
            if (!PropertyCanAddToExpression(dtoPropertyValue,dtoPropertyInfo.PropertyType))
            {
                return false;
            }
            
            if (dtoPropertyInfo.GetCustomAttribute<QueryFilterTypeAttribute>()!=null)
            {
                var queryFilterTypeAttribute = (QueryFilterTypeAttribute)dtoPropertyInfo.GetCustomAttribute(typeof(QueryFilterTypeAttribute));
                filterType = queryFilterTypeAttribute.FilterType;
                entityMatchPropertyInfo = entityPropertyInfos.FirstOrDefault(a => a.Name == queryFilterTypeAttribute.EntityName);
            }
            else
            {
                entityMatchPropertyInfo = entityPropertyInfos.FirstOrDefault(a => a.Name == dtoPropertyInfo.Name);
                filterType = BinaryExpressionFilterType.Equal;
            }

            // 属性不能映射，则不能生成表达式
            if (entityMatchPropertyInfo == null)
            {
                return false;
            }

            var left = Expression.Property(param, entityMatchPropertyInfo);//生成示例：a.Name
            var right = Expression.Constant(dtoPropertyValue, entityMatchPropertyInfo.PropertyType);//生成示例："zhangsang"
            filterExpression = GenerateFilterBinaryExpression(left, right, filterType);
            return true;
        }

        /// <summary>
        /// 此属性是否加入到表达式树
        /// </summary>
        /// <remarks>
        /// 1、所有的可为null的类型，如果值为null，则不加入表达式树，否则加入表达式
        /// 2、对int,datetime,guid等特殊的不可能为null的类型进行判断，如果是默认值，认为用户未对此属性有过滤操作，则不加入到表达式树。
        /// </remarks>
        /// <param name="value"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool PropertyCanAddToExpression(object propertyValue,Type propertyType)
        {
            if (propertyValue == null)
            {
                return false;
            }
            
            if ((propertyType== typeof(string)) && (string)propertyValue == string.Empty)
            {
                return false;
            }

            // 对int32,int64,bool,datetime,guid等不可能为null的类型进行判断。注意：下面这句不能判断string类型，因为string没有无参数的构造函数，会导致Activator.CreateInstance报错，所以要在此句上面对string类型进行处理。
            if ((propertyType != typeof(string)) && !propertyType.IsGenericType && Activator.CreateInstance(propertyType).Equals(propertyValue))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 生成过滤条件的表达式，如a.Name=="zhangsang"
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="binaryExpressionType"></param>
        /// <returns></returns>
        public static Expression GenerateFilterBinaryExpression(Expression left, Expression right, BinaryExpressionFilterType binaryExpressionType)
        {
            switch (binaryExpressionType)
            {
                case BinaryExpressionFilterType.Equal: return Expression.Equal(left, right);
                case BinaryExpressionFilterType.NotEqual: return Expression.NotEqual(left, right);
                case BinaryExpressionFilterType.LessThan: return Expression.LessThan(left, right);
                case BinaryExpressionFilterType.LessThanOrEqual: return Expression.LessThanOrEqual(left, right);
                case BinaryExpressionFilterType.GreaterThan: return Expression.GreaterThan(left, right);
                case BinaryExpressionFilterType.GreaterThanOrEqual: return Expression.GreaterThanOrEqual(left, right);
                case BinaryExpressionFilterType.Contain:
                    return Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right);
                default: return Expression.Equal(left, right);
            }
        }

        #region 弃用，保留代码
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <remarks>propertyExpressionTypes建议传入不对key做大小写区分的dic</remarks>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="entity"></param>
        ///// <param name="propertyExpressionTypes"></param>
        ///// <returns></returns>
        //[Obsolete]
        //public static Expression<Func<T, bool>> Generate<T>(T entity, Dictionary<string, BinaryExpressionFilterType> propertyExpressionTypes = null)
        //{
        //    // 可空类型的IsGenericType=true
        //    var properties = typeof(T).GetProperties().Where(a =>
        //        a.PropertyType.IsPublic && !a.PropertyType.IsAbstract &&
        //        !a.PropertyType.IsArray);
        //    var param = Expression.Parameter(typeof(T));
        //    var filtExpression = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
        //    foreach (var propertyInfo in properties)
        //    {
        //        var propertyValue = propertyInfo.GetValue(entity);
        //        if (PropertyCanAddToExpression(propertyValue, propertyInfo.PropertyType))
        //        {
        //            var left = Expression.Property(param, propertyInfo);//如：a.Name
        //            var right = Expression.Constant(propertyValue, propertyInfo.PropertyType);//如："zhangsang"
        //            filtExpression =
        //                Expression.And(GenerateFilterBinaryExpression(left, right, propertyInfo.Name, propertyExpressionTypes),
        //                    filtExpression);
        //        }
        //    }
        //    return Expression.Lambda<Func<T, bool>>(filtExpression, param);
        //}
        ///// <summary>
        ///// 生成过滤条件的表达式，如a.Name=="zhangsang"
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <param name="propertyName"></param>
        ///// <param name="propertyExpressionTypes"></param>
        ///// <returns></returns>
        //public static BinaryExpression GenerateFilterBinaryExpression(Expression left, Expression right, string propertyName,
        //    Dictionary<string, BinaryExpressionFilterType> propertyExpressionTypes = null)
        //{
        //    if (propertyExpressionTypes == null)
        //    {
        //        return Expression.Equal(left, right);
        //    }
        //    else
        //    {
        //        if (propertyExpressionTypes.TryGetValue(propertyName, out BinaryExpressionFilterType binaryExpressionType))
        //        {
        //            switch (binaryExpressionType)
        //            {
        //                case BinaryExpressionFilterType.Equal: return Expression.Equal(left, right);
        //                case BinaryExpressionFilterType.NotEqual: return Expression.NotEqual(left, right);
        //                case BinaryExpressionFilterType.LessThan: return Expression.LessThan(left, right);
        //                case BinaryExpressionFilterType.LessThanOrEqual: return Expression.LessThanOrEqual(left, right);
        //                case BinaryExpressionFilterType.GreaterThan: return Expression.GreaterThan(left, right);
        //                case BinaryExpressionFilterType.GreaterThanOrEqual: return Expression.GreaterThanOrEqual(left, right);
        //                default: return Expression.Equal(left, right);
        //            }
        //        }
        //        else
        //        {
        //            return Expression.Equal(left, right);
        //        }
        //    }
        //}


        #endregion
    }

    /// <summary>
    /// 二元表达式的运算类型，如:>,<,==,!=,contain等
    /// </summary>
    public enum BinaryExpressionFilterType
    {
        /// <summary>
        /// =
        /// <remarks>默认</remarks>
        /// </summary>
        Equal,
        /// <summary>
        /// !=
        /// </summary>
        NotEqual,
        /// <summary>
        /// <
        /// </summary>
        LessThan,
        /// <summary>
        /// <=
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// >
        /// </summary>
        GreaterThan,
        /// <summary>
        /// >=
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 字符串包含，string.Contains("xxx")
        /// </summary>
        Contain

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFilterTypeAttribute : Attribute
    {
        public QueryFilterTypeAttribute(string entityName, BinaryExpressionFilterType filterType)
        {
            EntityName = entityName;
            FilterType = filterType;
        }
        /// <summary>
        /// 对哪个字段时间查询
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// 查询的类型
        /// </summary>
        public BinaryExpressionFilterType FilterType { get; set; }
    }
}
