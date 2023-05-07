using GameManagement;
using UI;
using UnityEngine;

namespace Branch
{
    public class ResultDividedPart : MonoBehaviour
    {
        public float MoveToUiSpeed = 1f; // TODO: calculate this
        public float ResPartToIconDiff = 1f;
        private RectTransform _holder;
        private RectTransform _targetHolder;

        private void Awake()
        {
            var branchHolderPrefab = Resources.Load<RectTransform>("UI/BranchHolder");
            var canvasController = GameManager.Instance.CanvasController;
            _targetHolder = Instantiate(branchHolderPrefab, canvasController.gameObject.transform);

            _holder = Instantiate(branchHolderPrefab, canvasController.gameObject.transform);
            var rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.anchorMax = _holder.anchorMax;
            rectTransform.anchorMin = _holder.anchorMin;
            transform.SetParent(canvasController.gameObject.transform, true);

            _holder.anchoredPosition3D = rectTransform.anchoredPosition3D;
            rectTransform.SetParent(_holder, true);
        }

        private void Update()
        {
            MoveToUi(Time.deltaTime);
        }

        private void MoveToUi(float deltaTime)
        {
            var target = (_targetHolder.anchoredPosition3D - _holder.anchoredPosition3D).normalized;
            _holder.Translate(target * (MoveToUiSpeed * deltaTime));

            if (_holder.anchoredPosition.magnitude < ResPartToIconDiff)
                Destroy(_holder.gameObject);
        }
    }
}