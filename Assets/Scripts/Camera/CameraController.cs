using Arm;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        private float _cameraAspect;
        private float _initVerticalSize;
        private bool _isHorizontalMin;
        private ArmBase _maxArm;

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            _cameraAspect = _camera.aspect;

            var verticalSize = _camera.orthographicSize;
            var horizontalSize = _cameraAspect * verticalSize;
            _isHorizontalMin = verticalSize > horizontalSize;
            _initVerticalSize = verticalSize;
        }

        private void Update()
        {
            if (_maxArm == null)
            {
                _maxArm = FindObjectOfType<ArmBase>(); // TODO: change
                return;
            }

            if (_maxArm.IsDivided) return;

            var verticalSize = _camera.orthographicSize;
            var minSize = _isHorizontalMin ? _cameraAspect * verticalSize : verticalSize;
            var targetSize = Mathf.Max(minSize, _maxArm.Top.transform.localPosition.magnitude);
            _camera.orthographicSize =
                Mathf.Max(_initVerticalSize, _isHorizontalMin ? targetSize / _cameraAspect : targetSize);
        }
    }
}