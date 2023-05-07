using System.Linq;
using UnityEngine;

namespace Branch
{
    public abstract class TopBase : MonoBehaviour
    {
        [SerializeField] private float InitOffset;
        private Vector3 _initPos;

        private Bounds _totalBounds;
        public float Length => _totalBounds.size.y + InitOffset;

        public void Init(float scale)
        {
            _initPos = Vector3.up * InitOffset;

            transform.localScale = Vector3.one * scale;
            CalculateTotalBounds();
        }

        private void CalculateTotalBounds()
        {
            _totalBounds = GetComponentsInChildren<Renderer>().Select(r => r.bounds).Aggregate((b1, b2) =>
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