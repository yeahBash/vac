using System.Collections.Generic;
using Arm;
using UnityEngine;

namespace Body
{
    public class BodyBase : MonoBehaviour
    {
        private const float START_BRANCH_SIZE = 2f;
        private const float MAX_BRANCH_SIZE = 4f;
        public ArmBase Arm;

        public float Speed = 1f;
        public bool IsRotateOn = true;
        public bool IsBackground;

        public SpriteRenderer BodyRenderer;
        private readonly List<ArmBase> _arms = new List<ArmBase>();

        private void Start()
        {
            if (!IsBackground) Restart(true);
            BodyRenderer = GetComponent<SpriteRenderer>();
            BodyRenderer.material.SetFloat("_Length", Random.value);
        }

        private void Update()
        {
            if (IsRotateOn)
                transform.Rotate(Vector3.forward * (Speed * Time.deltaTime));
        }

        public ArmBase PlaceBranch(float angle, float size, bool isBackground)
        {
            var arm = Instantiate(Arm).GetComponentInChildren<ArmBase>();

            arm.gameObject.transform.SetParent(transform);
            arm.Size = size;
            arm.AnglePosition = angle;
            arm.IsBackground = isBackground;

            _arms.Add(arm);
            return arm;
        }

        private void PlaceBranches(int count, bool isSizeRandom)
        {
            for (var i = 0; i < count; i++)
                PlaceBranch(360f / count * i,
                    isSizeRandom ? START_BRANCH_SIZE + Random.value * MAX_BRANCH_SIZE : 
                        START_BRANCH_SIZE, IsBackground);
        }

        public void Clear()
        {
            foreach (var branch in _arms) Destroy(branch);
            _arms.Clear();
        }

        public void Restart(bool isRandom)
        {
            for (var i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
            PlaceBranches(isRandom ? (int)(Random.value * 10f) : 5, true);
        }
    }
}