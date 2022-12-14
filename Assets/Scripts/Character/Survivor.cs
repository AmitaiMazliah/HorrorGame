using UnityEngine;

namespace HorrorGame
{
    public class Survivor : Character
    {
        [SerializeField] private float runSpeedAddition = 10f;

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

        private void Start()
        {
            inputReader.EnableSurvivorInput();
            matchManager.RegisterSurvivor(this);
        }

        private void ToggleSprint(bool sprinting)
        {
            Logger.Info($"Toggle sprint {sprinting}");
            if (sprinting) movementSpeed += runSpeedAddition;
            else movementSpeed -= runSpeedAddition;
        }
    }
}