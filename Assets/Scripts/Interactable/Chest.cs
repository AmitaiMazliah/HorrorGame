using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class Chest : Interactable
    {
        public override bool IsAvailable(NetworkIdentity interactor)
        {
            var character = interactor.GetComponent<Character>();
            return base.IsAvailable(interactor) && character is Survivor;
        }
    }
}