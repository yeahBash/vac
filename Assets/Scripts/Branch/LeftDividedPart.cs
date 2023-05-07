using System;
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

        private void Update()
        {
            Shrink(Time.deltaTime);
        }

        public void Shrink(float deltaTime)
        {
            _growing.ChangeHeight(_growing.Length - deltaTime * ShrinkingSpeed);

            if (_growing.Length < 0) Destroy(gameObject);
        }
    }
}