using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public abstract class Interactable : NetworkBehaviour
    {
        protected static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] protected float interactionTime = 0f;

        [SyncVar] private bool available = true;
        
        [Command(requiresAuthority = false)]
        public void CmdInteract()
        {
            if (available)
            {
                logger.Info($"Interacting with {name}");
                available = false;
                
                InnerInteract();
            }
        }

        public virtual bool IsAvailable()
        {
            return available;
        }

        protected abstract void InnerInteract();
    }
}