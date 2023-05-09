using GameManagement;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        private float _initVerticalSize;
        private float VerticalSize => _camera.orthographicSize;
        private float HorizontalSize => _camera.aspect * VerticalSize;

        private void Awake()
        {
            if (FindObjectsOfType<CameraController>().Length > 1)
            {
                Debug.LogError("There are other camera controllers");
                return;
            }

            GameManager.Instance.InitCameraController(this);

            _camera = GetComponent<UnityEngine.Camera>();
            _initVerticalSize = VerticalSize;
        }

        public void ChangeSize(float targetSize)
        {
            var isHorizontalMin = VerticalSize > HorizontalSize;
            var minSize = isHorizontalMin ? HorizontalSize : VerticalSize;
            var resSize = Mathf.Max(minSize, targetSize);
            _camera.orthographicSize =
                Mathf.Max(_initVerticalSize, isHorizontalMin ? resSize / _camera.aspect : resSize);
        }

        public void ResetCamera()
        {
            _camera.orthographicSize = _initVerticalSize;
        }
    }
}