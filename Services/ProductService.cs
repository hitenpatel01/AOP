using AOP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOP.Models;
using AOP.Attributes;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace AOP.Services
{
    public class ProductService : BaseService, IProductService
    {
        private static IList<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Stock = 100 },
            new Product { Id = 2, Name = "Product 2", Stock = 200 },
            new Product { Id = 3, Name = "Product 3", Stock = 300 }
        };
        public ProductService(ILoggerFactory loggerFactory): base(loggerFactory)
        {
        }
        public virtual IList<Product> GetProducts()
        {
            return products;
        }
        [Cache]
        public virtual Product GetProductById(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }
        public virtual int DecrementStock(int id, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            product.Stock -= quantity;
            return product.Stock;
        }
    }
}
