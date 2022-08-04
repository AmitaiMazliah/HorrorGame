using Mirror;
using Sirenix.OdinInspector;
using Tempname.Input;
using UnityEngine;

namespace HorrorGame
{
    [HideNetworkBehaviourFields]
    public class CharacterSkillChecker : NetworkBehaviour
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] private InputReader inputReader;
        [SerializeField] private SkillCheckEventChannelSO skillEventStarted;

        private void OnEnable()
        {
            inputReader.skillCheckEvent += OnSkillCheckClick;
        }

        private void OnDisable()
        {
            inputReader.skillCheckEvent -= OnSkillCheckClick;
        }

        [TargetRpc]
        public void TargetStartSkillCheck(NetworkConnection target, SkillCheck skillCheck)
        {
            Logger.Info("Started skill check");
            skillEventStarted.RaiseEvent(skillCheck);
        }
        
        private void OnSkillCheckClick()
        {
            
        }
    }
}