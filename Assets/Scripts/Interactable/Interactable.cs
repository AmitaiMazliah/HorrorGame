using System;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HorrorGame
{
    public abstract class Interactable : NetworkBehaviour
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] private float progressSpeedPerInteractor = 0.1f;
        
        [Header("Skill checks")]
        public bool includeSkillChecks;
        [ShowIf(nameof(includeSkillChecks)), Range(0, 100)]
        public int skillCheckChance;

        [SyncVar]
        private bool available = true;
        [SyncVar, ProgressBar(0, 100)]
        public float progress;
        private readonly SyncHashSet<NetworkIdentity> interactors = new SyncHashSet<NetworkIdentity>();
        
        protected abstract void OnStartInteract();
        protected abstract void OnStopInteract();
        protected abstract void OnSuccessfulInteract();

        [ServerCallback]
        private void Update()
        {
            if (interactors.Count > 0)
            {
                Logger.Info($"Interactors count = {interactors.Count}");
                progress += progressSpeedPerInteractor * interactors.Count * Time.deltaTime;

                if (progress >= 100)
                {
                    SuccessfulInteract();
                }

                if (includeSkillChecks)
                {
                    foreach (var interactor in interactors)
                    {
                        var drawNumber = Random.Range(0, 100);
                        if (drawNumber <= skillCheckChance)
                        {
                            Logger.Info($"Start skill check");
                        }
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
                interactors.Add(sender.identity);
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
    }
}