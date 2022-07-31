using Mirror;
using UnityEditor;
using UnityEngine;

namespace HorrorGame
{
    public class CharacterHealth : NetworkBehaviour
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        [SerializeField] private float maxHealth = 100;

        [SyncVar] public float currentHealth;
        [SyncVar] public CharacterState state = CharacterState.Healthy;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        [ServerCallback]
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

        public void Hurt()
        {
            var previousState = state;
            state = state switch
            {
                CharacterState.Healthy => CharacterState.Injured,
                CharacterState.Injured => CharacterState.Dying,
                _ => state
            };
            logger.Info($"Character {name} changed {previousState} -> {state}");
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