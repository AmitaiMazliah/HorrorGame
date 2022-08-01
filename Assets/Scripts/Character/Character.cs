using System.Linq;
using Mirror;
using Tempname.Input;
using UnityEngine;

namespace HorrorGame
{
    public abstract class Character : NetworkBehaviour
    {
        protected static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] protected InputReader inputReader;

        [SerializeField] protected float movementSpeed = 5f;
        [SerializeField] private float interactingRadius = 5f;

        protected Rigidbody2D rb;
        private Collider2D myCollider;
        private Interactable interactable;

        protected virtual void OnEnable()
        {
            inputReader.moveEvent += OnMovement;
            inputReader.interactEvent += OnInteract;
        }

        protected virtual void OnDisable()
        {
            inputReader.moveEvent -= OnMovement;
            inputReader.interactEvent -= OnInteract;
        }
        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();
        }
        
        private void Update()
        {
            interactable = null;
            interactable = FindInteractable();

            if (interactable)
            {
                logger.Info($"Found interactable {interactable.name}");
            }
        }

        private void OnMovement(Vector2 movement)
        {
            rb.velocity = (movement * movementSpeed);
        }
        
        private void OnInteract()
        {
            if (interactable)
            {
                interactable.CmdInteract();
            }
        }
        
        private Interactable FindInteractable()
        {
            var position = transform.position;
            var results = new Collider2D[5];
            Physics2D.OverlapCircleNonAlloc(position, interactingRadius, results);

            return results.Where(c =>
                {
                    if (c != null && c != myCollider)
                    {
                        var interactable = c.GetComponentInChildren<Interactable>();
                        return interactable != null && interactable.IsAvailable(netIdentity);
                    }
                    return false;
                })
                .OrderBy(c => Vector3.Distance(position, c.transform.position))
                .Select(c => c.GetComponentInChildren<Interactable>())
                .FirstOrDefault();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactingRadius);
        }
    }
}