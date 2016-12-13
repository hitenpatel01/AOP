using AOP.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Interfaces
{
    public interface IOrderService
    {
        [Authenticate]
        bool PlaceOrder(int productId, int quantity);
    }
}
