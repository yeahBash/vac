using Branch;
using Branch.DividedParts;
using GameManagement;
using UnityEngine;

namespace Variants.Virus
{
    public class Spike : BranchBase
    {
        public TopBase TopPrefab;
        public float TopScale = 0.25f;

        private TopBase _top;
        public override float TotalLength => Length + _top.Length;

        protected override void Awake()
        {
            base.Awake();

            _top = Instantiate(TopPrefab, transform, false);
            _top.Init(TopScale);
        }

        public override bool Check(Vector2 pointToCheck, out Vector2 collisionPoint)
        {
            collisionPoint = Vector2.zero;

            var dot = Vector2.Dot(_top.transform.position.normalized, pointToCheck.normalized);
            if (Growing.Length > pointToCheck.magnitude && 1f - dot < DeadArea)
            {
                collisionPoint = pointToCheck;
                return true;
            }

            return false;
        }

        public override void Divide(Vector2 worldPoint)
        {
            var res = Growing.Length - worldPoint.magnitude;

            var resPart = CreatePart(worldPoint.magnitude, GrowingPartWidth, res,
                () => GameManager.Instance.LevelLoader.AddScore(res), typeof(MovingPart));
            _top.gameObject.transform.SetParent(resPart.transform, true);

            CreatePart(0f, GrowingPartWidth, worldPoint.magnitude, null, typeof(ShrinkingPart));

            IsDivided = true;

            Destroy(Growing.gameObject);
        }

        protected override void Grow(float length)
        {
            base.Grow(length);
            _top.SetOffset(length);
        }
    }
}