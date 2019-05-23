using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Pinger.Logger
{
    public sealed class NlogConfiguration: IConfigurationNlog
    {
        public LoggingConfiguration GetLogConfiguration(String logName)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget(logName);
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            config.AddTarget("file", fileTarget);
            consoleTarget.Layout = "${date:format=HH\\:mm\\:ss}| ${message}";
            fileTarget.Layout = "${date:format=HH\\:mm\\:ss}| ${message}";
            fileTarget.FileName = "${basedir}/logs/" + $"{logName}.txt";
            LoggingRule rule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(rule);
            LoggingRule rule3 =new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule3);
            FileTarget erorrTarget = new FileTarget(logName + "_errors");
            config.AddTarget("file", erorrTarget);
            erorrTarget.Layout = "${date:format=HH\\:mm\\:ss}| ${message}";
            erorrTarget.FileName = "${basedir}/logs/" + $"{logName}_errors.txt";
            LoggingRule rule2 = new LoggingRule("*", LogLevel.Error, erorrTarget);
            config.LoggingRules.Add(rule2);
            return config;
        }

    }
}
