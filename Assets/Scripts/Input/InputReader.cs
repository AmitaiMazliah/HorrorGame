using HorrorGame;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Tempname.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, PlayerControls.IPlayerActions, PlayerControls.IKillerActions, PlayerControls.ISurvivorActions
    {
        public event UnityAction<Vector2> moveEvent;
        public event UnityAction<Vector2> lookEvent;
        public event UnityAction<bool> interactEvent;
        public event UnityAction useItemEvent; 
        public event UnityAction attackEvent;
        public event UnityAction dashAttackEvent;
        public event UnityAction<bool> toggleSprintEvent; 

        private PlayerControls gameInput;

        private void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new PlayerControls();
                gameInput.Player.SetCallbacks(this);
                gameInput.Killer.SetCallbacks(this);
                gameInput.Survivor.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }
        
        public void EnableGameplayInput()
        {
            gameInput.Player.Enable();
        }

        public void DisableAllInput()
        {
            gameInput.Player.Disable();
            gameInput.Killer.Disable();
            gameInput.Survivor.Disable();
        }

        public void EnableKillerInput()
        {
            gameInput.Killer.Enable();
        }

        public void EnableSurvivorInput()
        {
            gameInput.Survivor.Enable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            moveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            lookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    interactEvent?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    interactEvent?.Invoke(false);
                    break;
            }
        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) useItemEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (context.duration <= Killer.DashAttackDuration) attackEvent?.Invoke();
                else dashAttackEvent?.Invoke();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    toggleSprintEvent?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    toggleSprintEvent?.Invoke(false);
                    break;
            }
        }
    }
}
