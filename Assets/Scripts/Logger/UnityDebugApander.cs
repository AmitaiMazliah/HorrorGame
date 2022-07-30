using NLog;
using NLog.Targets;
using UnityEngine;

[Target("UnityDebug")]
public class UnityDebugAppender : TargetWithLayout
{
    public UnityDebugAppender()
    {
        Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
    }

    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = Layout.Render(logEvent);

        if (logEvent.Level == LogLevel.Trace || logEvent.Level == LogLevel.Debug) Debug.LogAssertion(logMessage);
        else if (logEvent.Level == LogLevel.Info) Debug.Log(logMessage);
        else if (logEvent.Level == LogLevel.Warn) Debug.LogWarning(logMessage);
        else Debug.LogError(logMessage);
    }
}
