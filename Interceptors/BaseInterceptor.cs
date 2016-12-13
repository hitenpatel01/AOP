using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Interceptors
{
    public class BaseInterceptor
    {
        protected class InterceptionDetails
        {
            public string ClassName { get; set; }
            public string MethodName { get; set; }
            public string Arguments { get; set; }
        }
        protected ILogger logger;
        public BaseInterceptor(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger(this.GetType().Name);
        }
        protected InterceptionDetails GetInterceptionDetails(IInvocation invocation)
        {
            return new InterceptionDetails
            {
                ClassName = invocation.TargetType.Name,
                MethodName = invocation.Method.Name,
                Arguments = string.Join(", ", invocation.Arguments)
            };
        }
    }
}
