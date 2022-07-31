using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class CharacterHealth : NetworkBehaviour
    {
        [SerializeField] private float maxHealth = 100;

        [SyncVar] public float currentHealth;
        [SyncVar] public CharacterState state = CharacterState.Healthy;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (state == CharacterState.Dying)
            {
                currentHealth -= 1 * Time.deltaTime;
                if (currentHealth <= 0)
                {
                    state = CharacterState.Dead;
                }
            }
        }
    }

    public enum CharacterState : byte
    {
        Healthy,
        Injured,
        Dying,
        Dead
    }
}