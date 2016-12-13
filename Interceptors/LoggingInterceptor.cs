using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Interceptors
{
    public class LoggingInterceptor : BaseInterceptor, IInterceptor 
    {
        public LoggingInterceptor(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
        public void Intercept(IInvocation invocation)
        {
            var details = GetInterceptionDetails(invocation);
            logger.LogTrace($"Executing - {details.ClassName}.{details.MethodName}({details.Arguments})");
            invocation.Proceed();
            
            var result = null == invocation.ReturnValue ? 
                string.Empty : 
                (invocation.ReturnValue is ICollection ? 
                ($"{(invocation.ReturnValue as ICollection).Count} item(s)") : 
                invocation.ReturnValue.ToString());
            logger.LogTrace($"Executed - {details.ClassName}.{details.MethodName} => {result}");
        }
    }
}
