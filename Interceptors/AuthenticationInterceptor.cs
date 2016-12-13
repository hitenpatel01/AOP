using Castle.DynamicProxy;
using AOP.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AOP.Interceptors
{
    public class AuthenticationInterceptor : BaseInterceptor, IInterceptor
    {
        public AuthenticationInterceptor(ILoggerFactory loggerFactory): base(loggerFactory)
        {
        }
        public void Intercept(IInvocation invocation)
        {
            var isAuthenticationToBeChecked = invocation.Method.CustomAttributes.Any(ca => ca.AttributeType == typeof(AuthenticateAttribute));
            if (!isAuthenticationToBeChecked || (isAuthenticationToBeChecked && Thread.CurrentPrincipal.Identity.IsAuthenticated))
            {
                invocation.Proceed();
            }
            else
            {
                var details = GetInterceptionDetails(invocation);
                logger.LogTrace($"Access Denied - {details.ClassName}.{details.MethodName}({details.Arguments})");
                invocation.ReturnValue = invocation.Method.ReturnType.IsValueType ? Activator.CreateInstance(invocation.Method.ReturnType) : null;
            }
        }
    }
}
