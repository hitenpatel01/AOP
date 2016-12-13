using AOP.Attributes;
using AOP.Interfaces;
using AOP.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Services
{
    public class OrderService : BaseService, IOrderService
    {
        IProductService productService;
        public OrderService(ILoggerFactory loggerFactory, IProductService productService): base(loggerFactory)
        {
            this.productService = productService;
        }
        [Authenticate]
        public virtual bool PlaceOrder(int productId, int quantity)
        {
            productService.DecrementStock(productId, quantity);
            return true;
        }
    }
}
