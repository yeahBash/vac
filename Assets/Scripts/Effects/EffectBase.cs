using UnityEngine;

namespace Effects
{
    public abstract class EffectBase : MonoBehaviour
    {
        protected abstract void Activate();
        protected abstract void Deactivate();
    }
}