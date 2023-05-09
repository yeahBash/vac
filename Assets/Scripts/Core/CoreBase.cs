using System.Collections.Generic;
using System.Linq;
using Background;
using Branch;
using Destroyer;
using GameManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public abstract class CoreBase : MonoBehaviour
    {
        public BranchBase BranchPrefab;
        [HideInInspector] public DestroyerBase Destroyer; // TODO: temporary here, find better place
        public float RotationSpeed = 1f;
        public bool IsRotateOn = true;

        protected readonly List<BranchBase> ActiveBranches = new List<BranchBase>();

        private bool _isBackground;
        protected SpriteRenderer CoreRenderer;

        private Vector2 Size => CoreRenderer != null ? CoreRenderer.size : GetComponent<SpriteRenderer>().size;
        public float Radius => Size.magnitude / 2f;

        private float MaxTotalRadius =>
            Mathf.Max(Radius, ActiveBranches.Select(b => b.TotalLength).DefaultIfEmpty().Max());

        protected void Awake()
        {
            CoreRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Update()
        {
            if (IsRotateOn)
                Rotate();

            if (IsBackground) return;

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                AffectBranches(Time.deltaTime);
                ChangeCameraSize(MaxTotalRadius);
            }

            CheckBranches();
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.forward * (RotationSpeed * Time.deltaTime));
        }

        protected abstract void AffectBranches(float deltaTime);

        private void CheckBranches()
        {
            foreach (var branch in ActiveBranches)
            {
                var isCollided = branch.Check(Destroyer.transform.position, Destroyer.DeadArea, out var collisionPoint);
                if (isCollided) branch.Divide(collisionPoint);
            }

            ActiveBranches.RemoveAll(b => b.IsDivided);
        }

        private static void ChangeCameraSize(float targetSize)
        {
            GameManager.Instance.CameraController.ChangeSize(targetSize);
        }

        public void Init(IEnumerable<BranchBaseParameters> branchParameters,
            bool shouldCameraChange, bool isBackground)
        {
            PlaceBranches(branchParameters);
            if (shouldCameraChange) ChangeCameraSize(MaxTotalRadius);
            IsBackground = isBackground;
        }

        public IEnumerable<BranchBase> PlaceBranches(IEnumerable<BranchBaseParameters> branchParameters)
        {
            ClearBranches();
            foreach (var p in branchParameters)
                ActiveBranches.Add(PlaceBranch(p.AnglePosition, p.Length));

            return ActiveBranches;
        }

        private BranchBase PlaceBranch(float angle, float length)
        {
            var branch = Instantiate(BranchPrefab, transform).GetComponent<BranchBase>();
            branch.Length = length;
            branch.AnglePosition = angle;

            return branch;
        }

        public void ClearBranches()
        {
            foreach (var branch in ActiveBranches) Destroy(branch.gameObject);
            ActiveBranches.Clear();
        }

        #region Background

        private bool IsBackground
        {
            get => _isBackground;
            set
            {
                _isBackground = value;
                if (_isBackground) SetFadePanel();
            }
        }

        private void SetFadePanel()
        {
            // shouldn't change instructions order
            var coreRenderers = GetComponentsInChildren<SpriteRenderer>();
            var resource = Resources.Load<BackgroundFadePanel>("Background/FadePanel");
            var fadePanel = Instantiate(resource, transform);
            fadePanel.Init(MaxTotalRadius, transform.localScale.x); // get only x because it must be equal to y

            foreach (var coreRenderer in coreRenderers)
                coreRenderer.sortingOrder = fadePanel.SortingOrder - 1;
        }

        #endregion
    }
}