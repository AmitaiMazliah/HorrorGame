using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class CharacterHealth : Interactable
    {
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

        [Server]
        protected override void InnerInteract()
        {
            Heal();
        }

        [Server]
        private void Heal()
        {
            var previousState = state;
            state = state switch
            {
                CharacterState.Injured => CharacterState.Healthy,
                CharacterState.Dying => CharacterState.Injured,
                _ => state
            };
            logger.Info($"Character {name} changed {previousState} -> {state}");
        }

        public override bool IsAvailable(NetworkIdentity interactor)
        {
            var character = interactor.GetComponent<Character>();
            return base.IsAvailable(interactor) && character is Survivor &&
                   state is CharacterState.Injured or CharacterState.Dying;
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