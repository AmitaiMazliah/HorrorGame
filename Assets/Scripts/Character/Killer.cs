using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HorrorGame
{
    public class Killer : Character
    {
        public const float DashAttackDuration = 0.5f;

        [Header("Dash")]
        [SerializeField] private float dashStrength = 5f;
        [Header("Attack")]
        [SerializeField] private float attackRadius = 5f;
        [SerializeField] private Transform attackPosition;
        [SerializeField] private LayerMask survivorsLayer;

        private Camera cam;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputReader.attackEvent += OnAttack;
            inputReader.dashAttackEvent += OnDashAttack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            inputReader.attackEvent -= OnAttack;
            inputReader.dashAttackEvent -= OnDashAttack;
        }

        protected override void Awake()
        {
            base.Awake();
            cam = GetComponentInChildren<Camera>();
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
        
        private void OnDashAttack()
        {
            Logger.Info("Dash attacking");
            var target = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            rb.AddForce(target * dashStrength, ForceMode2D.Impulse);
            // rb.velocity = target * dashStrength;
            // CmdAttack();
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