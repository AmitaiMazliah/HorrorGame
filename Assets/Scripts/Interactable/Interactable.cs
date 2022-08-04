using System.Collections.Generic;
using System.Linq;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HorrorGame
{
    public abstract class Interactable : NetworkBehaviour
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [Header("Progress")]
        [SerializeField] private float progressSpeedPerInteractor = 0.1f;
        [SyncVar, ProgressBar(0, 100)]
        public float progress;
        
        [Header("Skill checks")]
        public bool includeSkillChecks;
        [ShowIf(nameof(includeSkillChecks)), Range(0, 100), SuffixLabel("%", Overlay = true)]
        public int skillCheckChance;
        [ShowIf(nameof(includeSkillChecks)), AssetList]
        [SerializeField] private SkillCheck[] skillChecks;

        [SerializeField] private float minTimeBetweenSkillChecks = 1f;

        [SyncVar]
        private bool available = true;

        private readonly SyncDictionary<NetworkIdentity, float> interactors =
            new SyncDictionary<NetworkIdentity, float>();

        [ServerCallback]
        private void Update()
        {
            if (interactors.Count <= 0) return;
            
            Logger.Info($"Interactors count = {interactors.Count}");
            progress += progressSpeedPerInteractor * interactors.Count * Time.deltaTime;

            if (progress >= 100)
            {
                SuccessfulInteract();
            }

            if (includeSkillChecks)
            {
                foreach (var interactor in interactors.Keys.ToList())
                {
                    var lastSkillCheckTime = interactors[interactor];
                    var drawNumber = Random.Range(0, 100);
                    if (drawNumber <= skillCheckChance && lastSkillCheckTime + minTimeBetweenSkillChecks <= Time.time)
                    {
                        Logger.Info($"Start skill check");
                        var skillCheck = skillChecks.GetRandom();
                        var skillChecker = interactor.GetComponent<CharacterSkillChecker>();
                        skillChecker.TargetStartSkillCheck(interactor.connectionToClient, skillCheck);
                        interactors[interactor] = Time.time + skillCheck.fullDuration;
                    }
                }
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdStartInteract(NetworkConnectionToClient sender = null)
        {
            if (sender != null)
            {
                Logger.Info($"Start interacting with {name}");
                interactors.Add(sender.identity, Time.time);
                OnStartInteract();
            }
        }
        
        [Command(requiresAuthority = false)]
        public void CmdStopInteract(NetworkConnectionToClient sender = null)
        {
            if (sender != null)
            {
                Logger.Info($"Stop interacting with {name}");
                interactors.Remove(sender.identity);
                OnStopInteract();
            }
        }

        public virtual bool IsAvailable(NetworkIdentity interactor)
        {
            return available;
        }

        private void SuccessfulInteract()
        {
            available = false;
            OnSuccessfulInteract();
            interactors.Clear();
        }
        
        protected virtual void OnStartInteract() {}
        protected virtual void OnStopInteract() {}
        protected virtual void OnSuccessfulInteract() {}
    }
}