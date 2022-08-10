using Mirror;
using Sirenix.OdinInspector;
using Tempname.Audio;
using Tempname.Events;
using Tempname.Input;
using UnityEngine;

namespace HorrorGame
{
    [HideNetworkBehaviourFields]
    public class CharacterSkillChecker : NetworkBehaviour
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] private InputReader inputReader;
        
        [Title("Audio")]
        [SerializeField] private AudioCueEventChannelSO sfxEventChannel;
        [SerializeField] private AudioConfigurationSO audioConfig;
        [SerializeField] private AudioCueSO startSkillCheckSound;
        
        [Title("Events")]
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
            // sfxEventChannel.RaisePlayEvent(startSkillCheckSound, audioConfig, transform.position);
            skillEventStarted?.RaiseEvent(skillCheck);
        }
        
        private void OnSkillCheckClick()
        {
            
        }
    }
}