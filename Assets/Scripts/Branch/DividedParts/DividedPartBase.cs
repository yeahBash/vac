using System;
using UnityEngine;

namespace Branch.DividedParts
{
    public abstract class DividedPartBase : MonoBehaviour
    {
        public Action OnDestroyed;
        protected GrowingBase Growing;

        protected virtual void Awake()
        {
            Growing = GetComponent<GrowingBase>();
        }

        protected void Update()
        {
            DoStep(Time.deltaTime);
        }

        protected abstract void DoStep(float deltaTime);

        protected virtual void DestroyPart()
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}