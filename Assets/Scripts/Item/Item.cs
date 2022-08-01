using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public abstract class Item : ScriptableObject
    {
        public virtual void Use(NetworkIdentity player)
        {
            Debug.Log($"Using {name}");
        }
    }
}