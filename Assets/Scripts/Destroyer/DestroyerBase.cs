using UnityEngine;
using UnityEngine.InputSystem;

namespace Destroyer
{
    public abstract class DestroyerBase : MonoBehaviour
    {
        public Vector2 MoveVector = -Vector2.up;
        public float Speed = 1f;
        public float DeadArea = 0.1f;
        public int MoveCount = 2;
        public InputAction TapAction;
        public InputAction SwipeAction;

        private int _currentMoveCount;

        // TODO: calculate target vector based on core pos
        private Vector2 _target;
        private Vector2 _origin;

        private bool _userInputPerformed;

        private void Awake()
        {
            ActivateInput();

            _origin = transform.position;
            _target = _origin + MoveVector;
        }

        private void OnDestroy()
        {
            DeactivateInput();
        }

        private void Update()
        {
            if (_currentMoveCount == 0) return;

            var curTarget = (_currentMoveCount - MoveCount) % 2 == 0 ? _target : _origin;
            MoveToTarget(curTarget, Speed, Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, DeadArea);
        }

        private void MoveToTarget(Vector2 target, float speed, float deltaTime)
        {
            Vector2 curPos = transform.position;
            var newPos = Vector2.MoveTowards(curPos, target, speed * deltaTime);
            if (newPos != curPos) transform.position = newPos;
            else _currentMoveCount--;
        }

        private void Move()
        {
            if (_currentMoveCount == 0)
                _currentMoveCount = MoveCount;
        }

        #region Input Methods
        private void ActivateInput()
        {
            TapAction.Enable();
            TapAction.performed += Move;
            SwipeAction.Enable();
            SwipeAction.performed += Move;
        }

        private void DeactivateInput()
        {
            TapAction.performed -= Move;
            TapAction.Disable();
            SwipeAction.performed -= Move;
            SwipeAction.Disable();
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            Move();
        }

        #endregion
    }
}