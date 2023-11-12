using UnityEngine;
using UnityEngine.EventSystems;

namespace Destroyer
{
    public abstract class DestroyerBase : MonoBehaviour
    {
        // TODO: calculate target vector based on core pos
        public Vector2 Target = Vector2.up;
        public float Speed = 1f;
        public float DeadArea = 0.1f;
        public int MoveCount = 2;

        private int _currentMoveCount;
        private Vector2 _origin;

        private void Awake()
        {
            _origin = transform.position;
        }

        private void Update()
        {
            if (_currentMoveCount != 0)
            {
                var curTarget = (_currentMoveCount - MoveCount) % 2 == 0 ? Target : _origin;
                MoveToTarget(curTarget, Speed, Time.deltaTime);
                return;
            }

            ProcessInput();
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

        private void ProcessInput()
        {
            if (!(Input.touchCount > 0)) return;

            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved &&
                !EventSystem.current.IsPointerOverGameObject(touch.fingerId) &&
                _currentMoveCount == 0)
                _currentMoveCount = MoveCount;
        }
    }
}