using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Service
{
    public class SnailBaseInterceptor : IInterceptor
    {
        protected static readonly MethodInfo HandleAsyncWithResultMethodInfo =
         typeof(SnailBaseInterceptor)
                 .GetMethod(nameof(InternalInterceptAsynchronousWithResult));

        protected MethodType methodType;
        protected bool HasReturnValue;

        public void Intercept(IInvocation invocation)
        {
            methodType = GetMethodType(invocation.Method.ReturnType);
            HasReturnValue = !(invocation.Method.ReturnType == typeof(void) || invocation.Method.ReturnType == typeof(Task));
            switch (methodType)
            {
                case MethodType.AsyncAction:
                    InterceptAsynchronous(invocation);
                    return;
                case MethodType.AsyncFunction:
                    CreateAsyncWithResultHandler(invocation.Method.ReturnType).Invoke(this, new object[] { invocation });
                    return;
                default:
                    InterceptSynchronous(invocation);
                    return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns>是否要继续执行后面的</returns>
        public virtual bool ExecuteBefore(IInvocation invocation)
        {
            return false;
        }

        public virtual void ExcecuteAfter(IInvocation invocation)
        {
            
        }


        /// <summary>
        /// 拦截同步
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            // Step 1. Do something prior to invocation.
            if (ExecuteBefore(invocation))
            {
                return;
            }

            invocation.Proceed();

            // Step 2. Do something after invocation.
            ExcecuteAfter(invocation);
        }

        /// <summary>
        /// 拦截异步无返回方法
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            // Step 1. Do something prior to invocation.
            if (ExecuteBefore(invocation))
            {
                return;
            }

            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task.ConfigureAwait(false);

            // Step 2. Do something after invocation.
            ExcecuteAfter(invocation);
        }

        /// <summary>
        /// 拦截异步有返回值方法
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TResult> InternalInterceptAsynchronousWithResult<TResult>(IInvocation invocation)
        {
            // Step 1. Do something prior to invocation.
            if (ExecuteBefore(invocation))
            {
                return await Task<TResult>.FromResult(default(TResult));
            }

            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            TResult result = await task.ConfigureAwait(false);

            // Step 2. Do something after invocation.
            ExcecuteAfter(invocation);

            return result;
        }



        //public void InterceptAsynchronousWithResult<TResult>(IInvocation invocation)
        //{
        //    invocation.ReturnValue = InternalInterceptAsynchronousWithResult<TResult>(invocation);
        //}
        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }
        public static MethodType GetMethodType(Type returnType)
        {
            if (returnType == typeof(void) || !typeof(Task).IsAssignableFrom(returnType))
                return MethodType.Synchronous;
            return returnType.GetTypeInfo().IsGenericType ? MethodType.AsyncFunction : MethodType.AsyncAction;
        }


        private static MethodInfo CreateAsyncWithResultHandler(Type returnType)
        {
            Type taskReturnType = returnType.GetGenericArguments()[0];
            MethodInfo method = HandleAsyncWithResultMethodInfo.MakeGenericMethod(taskReturnType);
            return method;
        }

       


        public enum MethodType
        {
            /// <summary>
            /// 同步方法，可能返回或是不返回值
            /// </summary>
            Synchronous,
            /// <summary>
            /// 返回task
            /// </summary>
            AsyncAction,
            /// <summary>
            /// 返回task<t>
            /// </summary>
            AsyncFunction,
        }

    }


  
}
