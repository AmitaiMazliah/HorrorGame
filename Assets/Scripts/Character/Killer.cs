using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

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
        }

        private void OnAttack()
        {
            logger.Info("Attacking");
            CmdAttack();
        }

        [Command(requiresAuthority = false)]
        private void CmdAttack()
        {
            var hitSurvivors = new Collider2D[4];
            Physics2D.OverlapCircleNonAlloc(attackPosition.position, attackRadius, hitSurvivors, survivorsLayer);

            foreach (var survivor in hitSurvivors)
            {
                if (survivor != null)
                {
                    logger.Info($"I hit someone {survivor.name}");
                    var health = survivor.GetComponent<CharacterHealth>();
                    health.Hurt();
                }
            }
        } 

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (attackPosition != null) Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
        }
    }
}