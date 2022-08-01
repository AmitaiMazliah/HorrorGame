using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class Killer : Character
    {
        [SerializeField] private float attackRadius = 5f;
        [SerializeField] private Transform attackPosition;
        [SerializeField] private LayerMask survivorsLayer;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputReader.attackEvent += OnAttack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            inputReader.attackEvent -= OnAttack;
        }

        private void Start()
        {
            inputReader.EnableKillerInput();
            matchManager.RegisterKiller(this);
        }

        private void OnAttack()
        {
            Logger.Info("Attacking");
            CmdAttack();
        }

        [Command(requiresAuthority = false)]
        private void CmdAttack()
        {
            var hitSurvivors = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, survivorsLayer);

            foreach (var survivor in hitSurvivors)
            {
                Logger.Info($"I hit someone {survivor.name}");
                var health = survivor.GetComponent<CharacterHealth>();
                health.Hurt();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (attackPosition != null) Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
        }
    }
}