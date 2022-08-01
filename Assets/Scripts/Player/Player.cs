using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private GameObject playerUi;
        
        [SyncVar] private string playerName;

        public override void OnStartLocalPlayer()
        {
            Instantiate(playerUi);
        }
    }
}