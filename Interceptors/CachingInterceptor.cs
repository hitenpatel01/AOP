using Castle.DynamicProxy;
using AOP.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Interceptors
{
    public class CachingInterceptor : BaseInterceptor, IInterceptor
    {
        Dictionary<string, object> cache;
        public CachingInterceptor(ILoggerFactory loggerFactory): base(loggerFactory)
        {
            this.cache = new Dictionary<string, object>();
        }
        public void Intercept(IInvocation invocation)
        {
            var isCacheable = invocation.Method.CustomAttributes.Any(ca => ca.AttributeType == typeof(CacheAttribute));
            var details = GetInterceptionDetails(invocation);
            var key = $"{details.ClassName}::{details.MethodName}::{details.Arguments}";
            if (isCacheable)
            {
                if(cache.ContainsKey(key))
                {
                    var cachedObject = cache[key];
                    invocation.ReturnValue = cachedObject;
                    logger.LogTrace($"Cache Hit - Key = \"{key}\"");
                }
                else
                {
                    invocation.Proceed();
                    cache[key] = invocation.ReturnValue;
                    logger.LogTrace($"Cache Miss - Key = \"{key}\"");
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
