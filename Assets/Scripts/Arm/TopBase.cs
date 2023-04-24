using UnityEngine;

namespace Arm
{
    public class TopBase : MonoBehaviour
    {
        [SerializeField] private float InitOffset;
        private Vector3 _originalTopPosition;

        public void Init(float scale)
        {
            transform.localPosition = Vector3.up * InitOffset;
            _originalTopPosition = transform.localPosition;
            transform.localScale = Vector3.one * scale;
        }

        public void SetOffset(float offset)
        {
            transform.localPosition = _originalTopPosition + offset * Vector3.up;
        }
    }
}
