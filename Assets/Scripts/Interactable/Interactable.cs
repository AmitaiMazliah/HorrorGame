using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public abstract class Interactable : NetworkBehaviour
    {
        [SerializeField] protected float interactionTime = 0f;
        
        [Command]
        public void CmdInteract()
        {
            
        }
    }
}