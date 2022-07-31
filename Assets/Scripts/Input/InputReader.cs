using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Tempname.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, PlayerControls.IPlayerActions
    {
        public event UnityAction<Vector2> moveEvent = delegate { };
        public event UnityAction<Vector2> lookEvent = delegate { };
        
        private PlayerControls gameInput;

        private void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new PlayerControls();
                gameInput.Player.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            lookEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void EnableGameplayInput()
        {
            gameInput.Player.Enable();
        }

        public void DisableAllInput()
        {
            gameInput.Player.Disable();
        }
    }
}
