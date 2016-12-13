using Castle.DynamicProxy;
using AOP.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP
{
    public static class CustomServiceCollectionExtensions
    {
        public static IServiceCollection AddTransientProxy<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var implementationType = typeof(TImplementation);
            return services.AddTransient<TService>(provider =>
            {
                var proxyGenerator = provider.GetService<ProxyGenerator>();
                var target = provider.GetService(implementationType);
                var interceptors = provider.GetServices<IInterceptor>();
                return provider
                    .GetService<ProxyGenerator>()
                    .CreateInterfaceProxyWithTarget(typeof(TService), target, interceptors.ToArray()) as TService;
            });
        }
    }
}
