using Microsoft.Extensions.Logging;
using PackageCopycat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Abstracts
{
    public abstract class LoggerBase : ILog
    {
        public ILogger<LoggerBase> Logger { get; set; }
        public LoggerBase(ILogger<LoggerBase> logger)
        {
            this.Logger = logger;
        }
    }
}
