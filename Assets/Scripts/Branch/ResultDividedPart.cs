using GameManagement;
using UI;
using UnityEngine;

namespace Branch
{
    public class ResultDividedPart : MonoBehaviour
    {
        public float MoveToUiSpeed = 1f;
        public float ResPartToIconDiff = 1f;
        private RectTransform _rectTransform;
        private RectTransform _targetBranchHolder;
        private CanvasController _canvas;

        private void Awake()
        {
            var branchHolderPrefab = Resources.Load<RectTransform>("UI/BranchHolder");
            _canvas = GameManager.Instance.CanvasController;
            _targetBranchHolder = Instantiate(branchHolderPrefab, _canvas.gameObject.transform);

            gameObject.AddComponent<RectTransform>();
        }

        private void Update()
        {
            MoveToUi(Time.deltaTime);
        }

        public void Init(GameObject top)
        {
            var topRectTransform = top.AddComponent<RectTransform>();
            _rectTransform = Instantiate(_targetBranchHolder, _canvas.gameObject.transform);
            _rectTransform.position = top.transform.position;

            top.transform.SetParent(_rectTransform, true);
            transform.SetParent(_rectTransform, true);
        }

        public void MoveToUi(float deltaTime)
        {
            if (_rectTransform == null) return;

            var target = (Vector3.zero - _rectTransform.anchoredPosition3D).normalized;
            _rectTransform.Translate(target * (MoveToUiSpeed * deltaTime));

            if (_rectTransform.anchoredPosition.magnitude < ResPartToIconDiff)
                Destroy(_rectTransform.gameObject);
        }
    }
}