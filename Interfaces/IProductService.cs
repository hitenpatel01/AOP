using AOP.Attributes;
using AOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Interfaces
{
    public interface IProductService
    {
        IList<Product> GetProducts();
        [Cache]
        Product GetProductById(int id);
        int DecrementStock(int id, int quantity);
    }
}
