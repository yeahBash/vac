using UnityEngine.InputSystem;

namespace Effects
{
    public abstract class TouchEffectBase : EffectBase
    {
        public InputAction HoldAction;
        public InputAction PositionAction;

        protected virtual void Awake()
        {
            ActivateInput();
        }

        private void OnDestroy()
        {
            DeactivateInput();
        }

        private void ActivateInput()
        {
            HoldAction.Enable();
            PositionAction.Enable();
            HoldAction.started += Started;
            HoldAction.performed += HoldOn;
            HoldAction.canceled += Cancelled;
        }

        protected abstract void Cancelled(InputAction.CallbackContext ctx);
        protected abstract void HoldOn(InputAction.CallbackContext ctx);
        protected abstract void Started(InputAction.CallbackContext ctx);

        private void DeactivateInput()
        {
            HoldAction.started -= Started;
            HoldAction.performed -= HoldOn;
            HoldAction.canceled -= Cancelled;
            HoldAction.Disable();
            PositionAction.Disable();
        }
    }
}