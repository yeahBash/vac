using System.Collections.Generic;
using System.Linq;
using Background;
using Branch;
using Destroyer;
using GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public abstract class CoreBase : MonoBehaviour
    {
        public BranchBase BranchPrefab;
        [HideInInspector] public DestroyerBase Destroyer; // TODO: temporary here, find better place
        public float RotationSpeed = 1f;
        public bool IsRotateOn = true;
        public InputAction HoldAction;

        protected readonly List<BranchBase> ActiveBranches = new List<BranchBase>();
        protected SpriteRenderer CoreRenderer;

        private Vector2 Size => CoreRenderer != null ? CoreRenderer.size : GetComponent<SpriteRenderer>().size;
        public float Radius => Size.magnitude / 2f;

        private float MaxTotalRadius =>
            Mathf.Max(Radius, ActiveBranches.Select(b => b.TotalLength).DefaultIfEmpty().Max());

        public bool IsInited { get; private set; }

        protected bool IsUserInputHold;

        protected void Awake()
        {
            CoreRenderer = GetComponent<SpriteRenderer>();

            if (IsBackground) return;
            ActivateInput();
        }

        private void OnDestroy()
        {
            if (IsBackground) return;
            DeactivateInput();
        }

        protected virtual void Update()
        {
            if (IsRotateOn)
                Rotate();
            
            if (IsBackground) return;

            if (IsUserInputHold)
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

        #region Input Methods

        private void ActivateInput()
        {
            HoldAction.Enable();
            HoldAction.performed += HoldOn;
            HoldAction.canceled += HoldOff;
        }
        private void DeactivateInput()
        {
            HoldAction.performed -= HoldOn;
            HoldAction.canceled -= HoldOff;
            HoldAction.Disable();
        }

        private void HoldOn(InputAction.CallbackContext ctx)
        {
            IsUserInputHold = true;
        }

        private void HoldOff(InputAction.CallbackContext ctx)
        {
            IsUserInputHold = false;
        }

        #endregion
    }
}