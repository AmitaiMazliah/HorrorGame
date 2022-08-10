using System;
using Mirror;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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

        private Interactable interactable;
        [OdinSerialize] public SkillCheck activeSkillCheck;
        private float skillCheckStartTime;

        private void OnEnable()
        {
            inputReader.skillCheckEvent += OnSkillCheckClick;
            skillEventStarted.OnEventRaised += OnSkillCheckStarted;
        }

        private void OnDisable()
        {
            inputReader.skillCheckEvent -= OnSkillCheckClick;
            skillEventStarted.OnEventRaised -= OnSkillCheckStarted;
        }

        [TargetRpc]
        public void TargetStartSkillCheck(NetworkConnection target, Interactable interactable)
        {
            var skillCheck = new SkillCheck
            {
                fullDuration = 5,
                startSafeTime = 4,
                goodDuration = 0.8f,
                excellentDuration = 0.1f
            };
            this.interactable = interactable;
            skillEventStarted?.RaiseEvent(skillCheck);
        }

        private void Update()
        {
            if (isLocalPlayer && activeSkillCheck != null && IsSkillCheckPassed())
            {
                Logger.Info("Skill check time passed, failing!");
                interactable.CmdSuccessSkillCheck();
            }
        }

        private bool IsSkillCheckPassed()
        {
            return Time.time > skillCheckStartTime + activeSkillCheck.startSafeTime + activeSkillCheck.goodDuration;
        }
        
        private void OnSkillCheckStarted(SkillCheck skillCheck)
        {
            Logger.Info("Started skill check");
            sfxEventChannel.RaisePlayEvent(startSkillCheckSound, audioConfig, transform.position);
            
            activeSkillCheck = skillCheck;
            skillCheckStartTime = Time.time;
        }

        private void OnSkillCheckClick()
        {
            if (Time.time < skillCheckStartTime + activeSkillCheck.startSafeTime ||
                Time.time > skillCheckStartTime + activeSkillCheck.goodDuration)
            {
                Logger.Info("User clicked skill check in wrong time, failing!");
                interactable.CmdFailSkillCheck();
            }
            else
            {
                Logger.Info("Successful skill check");
                interactable.CmdSuccessSkillCheck();
            }
        }
    }
}