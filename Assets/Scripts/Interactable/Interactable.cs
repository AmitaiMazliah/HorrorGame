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
        public void CmdInteract(NetworkConnectionToClient sender = null)
        {
            if (IsAvailable(sender.identity))
            {
                logger.Info($"Interacting with {name}");
                available = false;
                
                InnerInteract();
            }
        }

        public virtual bool IsAvailable(NetworkIdentity interactor)
        {
            return available;
        }

        protected abstract void InnerInteract();
    }
}