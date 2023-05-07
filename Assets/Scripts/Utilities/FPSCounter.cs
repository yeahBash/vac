using TMPro;
using UnityEngine;

namespace Utilities
{
    public class FPSCounter : MonoBehaviour
    {
        private int _framesCurrentSecond;
        private int _framesTotal;
        private int _lastRoundTime;

        private bool _presentationMode;
        private bool _skippingFrame = true;

        private float _totalTime;

        public float AvgFps;
        public int Fps;
        public float MaxFps;
        public float MinFps;

        public bool ResetAvgFps { get; set; }

        public TextMeshProUGUI DisplayText;

        private void Update()
        {
            if (ResetAvgFps)
            {
                ResetAvgFps = false;
                _totalTime = 0f;
                _lastRoundTime = 0;
                _framesTotal = 0;
                _framesCurrentSecond = 0;
                MinFps = float.MaxValue;
                MaxFps = -1f;
                _skippingFrame = true;
            }

            _totalTime += Time.deltaTime;
            _framesTotal++;
            _framesCurrentSecond++;

            if (_totalTime > _lastRoundTime)
            {
                _lastRoundTime = (int)_totalTime + 1;
                Fps = _framesCurrentSecond;
                _framesCurrentSecond = 0;

                if (_skippingFrame)
                {
                    _skippingFrame = false;
                }
                else
                {
                    AvgFps = _framesTotal / _totalTime;
                    MinFps = Mathf.Min(Fps, MinFps);
                    MaxFps = Mathf.Max(Fps, MaxFps);
                }
            }

            DisplayFps();
        }

        private void DisplayFps()
        {
            DisplayText.text = $"FPS: {Fps}\n" +
                               $"AvgFPS: {AvgFps}\n" +
                               $"MinFPS: {MinFps}\n" +
                               $"MaxFPS: {MaxFps}";
        }
    }
}