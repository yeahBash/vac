using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class SimpleTimerHandler : MonoBehaviour
    {
        public float Timer;
        public bool IsOn;
        public UnityEvent OnTimerIsUp;

        private float _initTimer;

        private void Awake()
        {
            _initTimer = Timer;
        }

        private void Update()
        {
            if (!IsOn) return;

            Timer -= Time.deltaTime;
            if (Timer < 0f)
            {
                OnTimerIsUp?.Invoke();
                StopTimer();
            }
        }

        public void PlayTimer()
        {
            IsOn = true;
        }

        public void StopTimer()
        {
            IsOn = false;
            Timer = _initTimer;
        }
    }
}