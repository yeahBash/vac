using System;
using Branch.DividedParts;
using UnityEngine;

namespace Branch
{
    public abstract class BranchBase : MonoBehaviour
    {
        public GrowingBase GrowingPrefab;
        public float GrowingPartWidth = 0.1f;

        public float DeadArea = 0.02f;
        public float GrowSpeed = 1f;

        private float _anglePosition;
        private float _length;

        protected GrowingBase Growing;

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

        public virtual float TotalLength => Length;

        public bool IsDivided { get; protected set; }

        protected virtual void Awake()
        {
            Growing = Instantiate(GrowingPrefab, transform, false);
            Growing.ChangeWidth(GrowingPartWidth);
        }

        public abstract bool Check(Vector2 pointToCheck, out Vector2 collisionPoint);

        public abstract void Divide(Vector2 worldPoint);

        protected virtual void Grow(float length)
        {
            Growing.Grow(length);
        }

        protected virtual GrowingBase CreatePart(float divisionPos, float width, float length, Action onDestroyed,
            params Type[] partComponents)
        {
            var part = Instantiate(GrowingPrefab, transform, false);

            part.transform.localPosition = Vector3.up * divisionPos;
            part.ChangeWidth(width);
            part.ChangeHeight(length);

            foreach (var partComponent in partComponents)
            {
                var dividedComponent = part.gameObject.AddComponent(partComponent) as DividedPartBase;
                if (dividedComponent != null) dividedComponent.OnDestroyed = onDestroyed;
            }

            return part;
        }
    }
}