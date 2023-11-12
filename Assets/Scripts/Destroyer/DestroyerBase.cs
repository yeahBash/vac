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

        private int _currentMoveCount;

        // TODO: calculate target vector based on core pos
        private Vector2 _target;
        private Vector2 _origin;

        private void Awake()
        {
            _origin = transform.position;
            _target = _origin + MoveVector;
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

        #region Button Handlers

        public void Move(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            if (_currentMoveCount == 0) _currentMoveCount = MoveCount;
        }

        #endregion
    }
}