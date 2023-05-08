using System;
using System.Collections.Generic;
using System.Linq;
using Branch.DividedParts;
using UnityEngine;
using Utilities;

namespace Branch
{
    public abstract class BranchBase : MonoBehaviour
    {
        public GrowingBase GrowingPrefab;
        public float GrowingPartWidth = 0.1f;
        public float GrowSpeed = 1f;

        private readonly List<GrowingBase> _parts = new List<GrowingBase>(2);

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

        public virtual bool Check(Vector2 destroyerPos, float deadArea, out Vector2 collisionPoint)
        {
            collisionPoint = Vector2.zero;

            var distance = MathHelper.GetNormal(destroyerPos, GetPointToCheck(), out var projMultiplier).magnitude;
            if (Growing.Length > destroyerPos.magnitude && distance - GrowingPartWidth / 2f < deadArea &&
                projMultiplier > 0)
            {
                collisionPoint = destroyerPos;
                return true;
            }

            return false;
        }

        public abstract void Divide(Vector2 worldPoint);

        protected virtual Vector2 GetPointToCheck()
        {
            return transform.position + transform.TransformPoint(Vector3.up * Length);
        }

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

            _parts.Add(part);
            return part;
        }

        protected void OnDestroy()
        {
            foreach (var part in _parts.Where(p => p != null))
                Destroy(part.gameObject);
        }
    }
}