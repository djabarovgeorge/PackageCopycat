using Microsoft.Extensions.Logging;
using PackageCopycat.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Models
{
    class LoggerSuper : LoggerBase
    {
        public LoggerSuper(ILogger<LoggerBase> logger) : base(logger)
        {
        }
    }
}
