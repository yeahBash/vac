using System;
using UnityEngine;

namespace Arm
{
    public class ArmBase : MonoBehaviour
    {
        public TopBase TopPrefab;
        public GrowingBase GrowingPrefab;
        public float GrowingPartWidth = 0.1f;
        public float TopScale = 0.25f;

        public bool IsBackground;
        public float DeadArea = 0.02f;
        public float GrowSpeed = 1f;

        private GameObject _destroyer;
        private TopBase _top;
        private GrowingBase _growing;

        private float _anglePosition;
        private bool _isDivided;
        private float _length;

        public float AnglePosition
        {
            get => _anglePosition;
            set
            {
                _anglePosition = value;
                transform.localRotation = Quaternion.Euler(Vector3.back * _anglePosition);
            }
        }

        public float Length
        {
            get => _length;
            set
            {
                _length = value;
                Grow(_length);
            }
        }

        private void Awake()
        {
            _destroyer = GameObject.FindGameObjectWithTag("Destroyer");
        }

        private void Start()
        {
            _top = Instantiate(TopPrefab, transform, false);
            _top.Init(TopScale);

            _growing = Instantiate(GrowingPrefab, transform, false);
            _growing.ChangeWidth(GrowingPartWidth);
        }

        private void Update()
        {
            if (_isDivided) return;

            if (Input.GetMouseButton(0) && !IsBackground)
                Length += GrowSpeed * Time.deltaTime;

            var isCollided = Check(out var point);
            if (isCollided)
                Divide(point);
        }

        public void Divide(Vector2 worldPoint)
        {
            var res = _growing.Length - worldPoint.magnitude;

            CreatePart(worldPoint.magnitude, Vector3.zero, res);
            CreatePart(worldPoint.magnitude, Vector3.forward * 180f, worldPoint.magnitude);

            _isDivided = true;

            Destroy(_growing.gameObject);
        }

        private bool Check(out Vector2 collisionPoint)
        {
            var dot = Vector2.Dot(_top.transform.position.normalized, _destroyer.transform.position.normalized);

            if (_growing.Length > _destroyer.transform.position.magnitude && 1f - dot < DeadArea)
            {
                collisionPoint = _destroyer.transform.position;
                return true;
            }

            collisionPoint = Vector2.zero;
            return false;
        }

        private void Grow(float length)
        {
            _growing.Grow(length);
            _top.SetOffset(length);
        }

        private void CreatePart(float divisionPos, Vector3 eulerRot, float length)
        {
            var part = Instantiate(GrowingPrefab, transform, false);

            part.transform.localPosition = Vector3.up * divisionPos;
            part.transform.localRotation = Quaternion.Euler(eulerRot);
            part.ChangeWidth(GrowingPartWidth);
            part.ChangeHeight(length);
        }
    }

    [Serializable]
    public struct ArmParameters
    {
        public float Size;
        public float AnglePosition;
    }
}