using NLog;
using NLog.Config;
using NLog.Layouts;
using UnityEngine;

public static class LoggingConfig
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void Config()
    {
        var config = new LoggingConfiguration();

        var target = new UnityDebugAppender();
        target.Layout = new SimpleLayout("${level:uppercase=true}|${logger}|${message}");

        config.AddRule(LogLevel.Info, LogLevel.Fatal, target);

        LogManager.Configuration = config;
    }
}
