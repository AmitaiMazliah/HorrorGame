using System;
using System.Linq;
using UnityEngine;

namespace HorrorGame
{
    public class Survivor : Character
    {
        [SerializeField] private float runSpeedAddition = 10f;
        [SerializeField] private float interactingRadius = 5f;

        private Collider2D myCollider;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputReader.toggleSprintEvent += ToggleSprint;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            inputReader.toggleSprintEvent -= ToggleSprint;
        }

        protected override void Awake()
        {
            base.Awake();
            myCollider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            inputReader.EnableSurvivorInput();
        }

        private void Update()
        {
            var interactable = FindInteractable();

            if (interactable)
            {
                logger.Info($"Found interactable {interactable.name}");
            }
        }

        private void ToggleSprint(bool sprinting)
        {
            logger.Info($"Toggle sprint {sprinting}");
            if (sprinting) movementSpeed += runSpeedAddition;
            else movementSpeed -= runSpeedAddition;
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
                        return interactable != null && interactable.IsAvailable();
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