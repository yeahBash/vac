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

        protected List<BranchBase> ActiveBranches { get; } = new List<BranchBase>();
        protected SpriteRenderer CoreRenderer;

        private Vector2 Size => CoreRenderer != null ? CoreRenderer.size : GetComponent<SpriteRenderer>().size;
        public float Radius => Size.magnitude / 2f;

        private float MaxTotalRadius =>
            Mathf.Max(Radius, ActiveBranches.Select(b => b.TotalLength).DefaultIfEmpty().Max());

        private bool IsInited { get; set; }

        public bool AreBranchesOver => ActiveBranches.Count == 0; //TODO: this should consider parts

        protected void Awake()
        {
            CoreRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            if (IsRotateOn)
                Rotate();

            if (IsBackground) return;

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) // TODO: fix for mobile devices
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

        public void Init(IEnumerable<BranchBaseParameters> branchParameters, DestroyerBase destroyer,
            bool shouldCameraChange, bool isBackground)
        {
            Destroyer = destroyer;
            PlaceBranches(branchParameters);
            if (shouldCameraChange) ChangeCameraSize(MaxTotalRadius);
            IsBackground = isBackground;

            IsInited = true;
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

        //TODO: think about better place
        #region Background

        private bool _isBackground;

        private bool IsBackground
        {
            get => _isBackground;
            set
            {
                _isBackground = value;
                var existingMarker = gameObject.GetComponent<BackgroundMarker>();

                if (_isBackground)
                {
                    if (existingMarker == null)
                        existingMarker = gameObject.AddComponent<BackgroundMarker>();

                    existingMarker.SetFadePanel(MaxTotalRadius);
                } else
                {
                    if (existingMarker != null) Destroy(existingMarker);
                }
            }
        }

        #endregion
    }
}