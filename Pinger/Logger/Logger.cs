using System;
using NLog;

namespace Pinger.Logger
{
    class Logger:ILogger<String>
    {
        private readonly NLog.Logger _logger;

        public Logger(String logName, IConfigurationNlog configuration)
        {
            var name = String.IsNullOrEmpty(logName) ? DateTime.Now.ToShortDateString() : logName;
            _logger = LogManager.GetCurrentClassLogger();
            LogManager.Configuration = configuration.GetLogConfiguration(name);
        }
        public void Write(String value)
        {
            _logger.Info(value);
            _logger.Debug(value);
        }
    }
}
