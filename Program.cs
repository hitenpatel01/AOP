using System;
using Microsoft.Extensions.DependencyInjection;
using AOP.Services;
using AOP.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Primitives;
using Castle.DynamicProxy;
using AOP.Interceptors;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Extensions.Logging;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;

namespace AOP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureServices();

            //CCC - Logging
            DemonstrateLogging();

            //CCC - Caching
            DemonstrateCaching();

            ///CCC - Security
            DemonstrateSecurity();

            //Performance
            MeasurePerformance();

            Console.WriteLine("Press ANY key to quit...");
            Console.ReadKey();
        }
        public static void DemonstrateLogging()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var products = productService.GetProducts();            //First call, cache miss
        }
        public static void DemonstrateCaching()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var product = productService.GetProductById(1);         //First call, cache miss
            var productAgain = productService.GetProductById(1);    //Subsequent call, cache hit
        }
        public static void DemonstrateSecurity()
        {
            var orderManager = ServiceProvider.GetService<IOrderService>();
            orderManager.PlaceOrder(1, 12);                         //Unauthenticated Request

            Thread.CurrentPrincipal = new GenericPrincipal(         //Authenticate
                new GenericIdentity("Test"), null);

            orderManager.PlaceOrder(1, 12);                         //Authenticated Request
        }
        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            //Configure Logging
            services.AddSingleton<ILoggerFactory>(new LoggerFactory()
                .AddNLog());
            LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
            
            //Configure Interceptors
            services.AddSingleton<ProxyGenerator>(new ProxyGenerator());
            services.AddSingleton<IInterceptor, LoggingInterceptor>();
            services.AddSingleton<IInterceptor, AuthenticationInterceptor>();
            services.AddSingleton<IInterceptor, CachingInterceptor>();

            //Configure Services
            services.AddTransient<ProductService>();
            services.AddTransientProxy<IProductService, ProductService>();
            services.AddTransient<OrderService>();
            services.AddTransientProxy<IOrderService, OrderService>();

            ServiceProvider = services.BuildServiceProvider();
            //Logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(Program).Name);
        }
        public static void MeasurePerformance()
        {
            //No IOC
            var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            Test("Test - No IOC", null, loggerFactory);
            
            //IOC without interception
            var testIOCCollection = new ServiceCollection();
            testIOCCollection.AddSingleton<ILoggerFactory>(loggerFactory);
            testIOCCollection.AddTransient<IProductService, ProductService>();
            var testIOC = testIOCCollection.BuildServiceProvider();
            Test("Test - IOC without interception", testIOC, loggerFactory);

            //IOC with interception
            Test("Test - IOC with interception", ServiceProvider, loggerFactory);
        }

        public static void Test(string testName, IServiceProvider ioc, ILoggerFactory loggerFactory)
        {
            Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger(testName);
            var productService = (null != ioc) ? ioc.GetService<IProductService>() : new ProductService(loggerFactory);
            var stopWatch = Stopwatch.StartNew();
            int totalCount = 1000000;
            for (int cnt = 0; cnt < totalCount; cnt++)
            {
                productService = (null != ioc) ? ioc.GetService<IProductService>() : new ProductService(loggerFactory);
            }
            stopWatch.Stop();
            logger.LogInformation($"Tests: {totalCount}, Duration: {stopWatch.ElapsedMilliseconds} msec");
        }
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
