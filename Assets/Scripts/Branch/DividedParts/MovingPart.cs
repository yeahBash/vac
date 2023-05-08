using GameManagement;
using UI;
using UI.Screens;
using UnityEngine;

namespace Branch.DividedParts
{
    public class MovingPart : DividedPartBase
    {
        public float MoveToUiSpeed = 2f; // TODO: calculate this
        public float ResPartToIconDiff = 1f;
        private CanvasController _canvasController;
        private LevelUI _levelScreen;
        private RectTransform _rectTransform;
        private RectTransform _targetIcon;

        protected override void Awake()
        {
            base.Awake();

            _canvasController = GameManager.Instance.CanvasController;
            _levelScreen = _canvasController.CurrentScreen as LevelUI;
            if (_levelScreen == null) return;

            Init();
        }

        protected override void DoStep(float deltaTime)
        {
            MoveToUi(deltaTime);
        }

        private void Init()
        {
            _targetIcon = _levelScreen.ScoreIcon.GetComponent<RectTransform>();
            _rectTransform = gameObject.AddComponent<RectTransform>();

            _rectTransform.anchorMax = _targetIcon.anchorMax;
            _rectTransform.anchorMin = _targetIcon.anchorMin;
            _rectTransform.pivot = _targetIcon.pivot;

            _rectTransform.SetParent(_canvasController.gameObject.transform, true);
        }

        private void MoveToUi(float deltaTime)
        {
            var targetDir = _targetIcon.anchoredPosition3D - _rectTransform.anchoredPosition3D;
            _rectTransform.Translate(targetDir.normalized * (MoveToUiSpeed * deltaTime));

            if (targetDir.magnitude < ResPartToIconDiff)
                DestroyPart();
        }
    }
}