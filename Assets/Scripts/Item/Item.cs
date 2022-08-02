using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class Item : ScriptableObject
    {
        public virtual void Use(NetworkIdentity player)
        {
            Debug.Log($"Using {name}");
        }
    }
}