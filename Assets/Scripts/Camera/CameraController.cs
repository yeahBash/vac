using System;
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
        public Action<float> OnCameraSizeChanged;

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
            var size = Mathf.Max(_initVerticalSize, isHorizontalMin ? resSize / _camera.aspect : resSize);
            SetCameraOrthographicSize(size);
        }

        public void ResetCamera()
        {
            SetCameraOrthographicSize(_initVerticalSize);
        }

        private void SetCameraOrthographicSize(float size)
        {
            _camera.orthographicSize = size;
            OnCameraSizeChanged?.Invoke(VerticalSize / _initVerticalSize);
        }

        public Vector3 ScreenToWorldPoint(Vector3 screenPos)
        {
            return _camera.ScreenToWorldPoint(screenPos);
        }
    }
}