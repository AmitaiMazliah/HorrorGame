using System.Diagnostics;
using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// This class is used for Events that have a bool argument.
/// Example: An event to toggle a UI interface
/// </summary>
[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : DescriptionBaseSO
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public event UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
#if UNITY_EDITOR
        var method = new StackFrame(1).GetMethod();
        var callingClassName = method.DeclaringType?.Name;
        var callingFuncName = method.Name;
        logger.Info($"Event {name} has been raised by {callingClassName}.{callingFuncName}");
#endif
        OnEventRaised?.Invoke(value);
    }

    [Button(ButtonSizes.Medium, ButtonStyle.Box, Expanded = true)]
    private void TestEvent(bool value)
    {
        RaiseEvent(value);
    }
}
