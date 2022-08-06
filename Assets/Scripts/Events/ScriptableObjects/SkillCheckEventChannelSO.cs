using System.Diagnostics;
using HorrorGame;
using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Events/Skill Check Event Channel")]
public class SkillCheckEventChannelSO : DescriptionBaseSO
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public event UnityAction<SkillCheck> OnEventRaised;

    public void RaiseEvent(SkillCheck value)
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
    private void TestEvent(SkillCheck value)
    {
        RaiseEvent(value);
    }
}
