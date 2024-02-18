using Camera;
using GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Effects
{
    public class ElectricityEffect : TouchEffectBase
    {
        public ParticleSystem Particles;
        private Vector3 _initEffectScale;

        private CameraController CameraController => GameManager.Instance.CameraController;

        protected override void Awake()
        {
            base.Awake();
            _initEffectScale = Particles.gameObject.transform.localScale;
        }

        private void Update()
        {
            if (PositionAction.triggered) SetParticlesPosition();
        }

        protected override void Activate()
        {
            CameraController.OnCameraSizeChanged += SetParticlesScale;
            Particles.gameObject.SetActive(true);
        }

        protected override void Deactivate()
        {
            CameraController.OnCameraSizeChanged -= SetParticlesScale;
            Particles.gameObject.SetActive(false);
        }

        private void SetParticlesPosition()
        {
            var screenPos = PositionAction.ReadValue<Vector2>();
            var worldPoint = CameraController.ScreenToWorldPoint(screenPos);
            Particles.transform.position = worldPoint;
        }

        private void SetParticlesScale(float cameraChangeCoefficient)
        {
            Particles.transform.localScale = _initEffectScale * cameraChangeCoefficient;
        }

        protected override void Cancelled(InputAction.CallbackContext ctx)
        {
            Deactivate();
        }

        protected override void HoldOn(InputAction.CallbackContext ctx)
        {
            Activate();
        }

        protected override void Started(InputAction.CallbackContext ctx)
        {
        }
    }
}