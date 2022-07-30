using NLog;
using NLog.Config;
using UnityEngine;

public static class LoggingConfig
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void Config()
    {
        var config = new LoggingConfiguration();

        var target = new UnityDebugAppender();

        config.AddRule(LogLevel.Info, LogLevel.Fatal, target);

        LogManager.Configuration = config;
    }
}
