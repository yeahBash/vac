using System.Linq;
using UnityEngine;

namespace Branch
{
    public abstract class TopBase : MonoBehaviour
    {
        [SerializeField] private float InitOffset;
        public float TopScale = 0.25f;

        private Vector3 _initPos;

        private Bounds _totalBounds;
        public float Length => (_totalBounds.size.y + InitOffset) * transform.localScale.y;

        protected void Awake()
        {
            Init(TopScale, InitOffset);
        }

        protected virtual void Init(float scale, float offset)
        {
            _initPos = Vector3.up * offset;

            transform.localScale = Vector3.one * scale;
            _totalBounds = CalculateTotalBounds();
        }

        private Bounds CalculateTotalBounds()
        {
            return GetComponentsInChildren<Renderer>().Select(r => r.localBounds).Aggregate((b1, b2) =>
            {
                var res = b1;
                res.Encapsulate(b2);
                return res;
            });
        }

        public void SetOffset(float offset)
        {
            transform.localPosition = _initPos + offset * Vector3.up;
        }
    }
}