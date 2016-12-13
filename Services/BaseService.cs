using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AOP.Services
{
    public class BaseService
    {
        ILogger logger;
        public BaseService(ILoggerFactory loggerFactor)
        {
            logger = loggerFactor.CreateLogger(this.GetType().Name);
        }
    }
}
