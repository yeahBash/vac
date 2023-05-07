using UnityEngine;

namespace Branch
{
    public class LeftDividedPart : MonoBehaviour
    {
        public float ShrinkingSpeed = 4f;
        private GrowingBase _growing;

        private void Awake()
        {
            _growing = GetComponent<GrowingBase>();
        }

        public void Shrink(float deltaTime)
        {
            if (_growing.Length >= 0)
                _growing.ChangeHeight(_growing.Length - deltaTime * ShrinkingSpeed);
        }
    }
}