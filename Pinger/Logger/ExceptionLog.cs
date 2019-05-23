using System;
using NLog;

namespace Pinger.Logger
{
    class ExceptionLogger : ILogger<Exception>
    {
        private readonly NLog.Logger _logger;

        public ExceptionLogger(String logName, IConfigurationNlog configuration)
        {
            var name = String.IsNullOrEmpty(logName) ? DateTime.Now.ToShortDateString() : logName;
            _logger = LogManager.GetCurrentClassLogger();
            LogManager.Configuration = configuration.GetLogConfiguration(name);
        }

        public void Write(Exception value)
        {
            _logger.Error(value);
        }
    }
}
    
