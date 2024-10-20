using Branch;
using Branch.DividedParts;
using Effects;
using GameManagement;
using UnityEngine;

namespace Variants.Virus
{
    public class Spike : BranchBase
    {
        public TopBase TopPrefab;
        public CutEffectBase CutEffectPrefab;
        private TopBase _top;

        public override float TotalLength => Length + _top.Length;

        protected override void Awake()
        {
            base.Awake();
            _top = Instantiate(TopPrefab, transform, false);
        }

        protected override Vector2 GetPointToCheck()
        {
            return _top.transform.position;
        }

        public override void Divide(Vector2 worldPoint)
        {
            var res = Growing.Length - worldPoint.magnitude;

            var resPart = CreatePart(worldPoint.magnitude, GrowingPartWidth, res,
                () => AddScore(res), typeof(MovingPart));
            _top.gameObject.transform.SetParent(resPart.transform, true);

            CreatePart(0f, GrowingPartWidth, worldPoint.magnitude, null, typeof(ShrinkingPart));
            var cutEffect = Instantiate(CutEffectPrefab, worldPoint, Quaternion.identity);
            IsDivided = true;

            Destroy(Growing.gameObject);
        }

        private void AddScore(float res)
        {
            GameManager.Instance.LevelLoader.AddScore(res);
        }

        protected override void Grow(float length)
        {
            base.Grow(length);
            _top.SetOffset(length);
        }
    }
}