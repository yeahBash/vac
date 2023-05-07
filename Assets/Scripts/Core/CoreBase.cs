using System.Collections.Generic;
using System.Linq;
using Branch;
using Destroyer;
using GameManagement;
using UnityEngine;

namespace Core
{
    public abstract class CoreBase : MonoBehaviour
    {
        public BranchBase BranchPrefab;
        [HideInInspector] public DestroyerBase Destroyer; // TODO: temporary here, find better place
        public float RotationSpeed = 1f;
        public bool IsRotateOn = true;

        public bool IsBackground;

        protected readonly List<BranchBase> Branches = new List<BranchBase>();
        protected SpriteRenderer CoreRenderer;

        protected void Awake()
        {
            CoreRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Update()
        {
            if (IsRotateOn)
                Rotate();

            if (Input.GetMouseButton(0) && !IsBackground)
            {
                AffectBranches(Time.deltaTime);
                ChangeCameraSettings();
            }

            CheckBranches();
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.forward * (RotationSpeed * Time.deltaTime));
        }

        private void ChangeCameraSettings()
        {
            var maxTotalLength = Branches.Select(b => b.TotalLength).Max();
            GameManager.Instance.CameraController.ChangeSize(maxTotalLength);
        }

        protected abstract void AffectBranches(float deltaTime);

        private void CheckBranches()
        {
            foreach (var branch in Branches)
            {
                if (branch.IsDivided) continue;

                var isCollided = branch.Check(Destroyer.transform.position, out var collisionPoint);
                if (isCollided)
                {
                    branch.Divide(collisionPoint, out var res);
                    GameManager.Instance.LevelLoader.AddScore(res);
                }
            }
        }

        public IEnumerable<BranchBase> PlaceBranches(IEnumerable<BranchBaseParameters> branchParameters)
        {
            foreach (var p in branchParameters)
                Branches.Add(PlaceBranch(p.AnglePosition, p.Length));

            return Branches;
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
            foreach (var branch in Branches) Destroy(branch.gameObject);
            Branches.Clear();
        }
    }
}