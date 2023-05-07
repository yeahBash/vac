using UnityEngine;

namespace Branch
{
    public abstract class BranchBase : MonoBehaviour
    {
        public TopBase TopPrefab;
        public GrowingBase GrowingPrefab;
        public float GrowingPartWidth = 0.1f;
        public float TopScale = 0.25f;

        public float DeadArea = 0.02f;
        public float GrowSpeed = 1f;

        [HideInInspector] public TopBase Top;

        private float _anglePosition;
        private GrowingBase _growing;
        private LeftDividedPart _leftPart;
        private float _length;
        private ResultDividedPart _resPart;

        public float AnglePosition
        {
            get => _anglePosition;
            set
            {
                _anglePosition = value;
                transform.localRotation = Quaternion.Euler(Vector3.back * _anglePosition);
            }
        }

        public float Length
        {
            get => _length;
            set
            {
                _length = value;
                Grow(_length);
            }
        }

        public float TotalLength => Length + Top.Length;

        public bool IsDivided { get; private set; }

        protected void Awake()
        {
            Top = Instantiate(TopPrefab, transform, false);
            Top.Init(TopScale);

            _growing = Instantiate(GrowingPrefab, transform, false);
            _growing.ChangeWidth(GrowingPartWidth);
        }

        protected void Update()
        {
            if (IsDivided)
            {
                _resPart.MoveToUi(Time.deltaTime);
                _leftPart.Shrink(Time.deltaTime);
            }
        }

        public void Divide(Vector2 worldPoint, out float res)
        {
            res = _growing.Length - worldPoint.magnitude;

            var resPart = CreatePart(worldPoint.magnitude, GrowingPartWidth, res);
            _resPart = resPart.gameObject.AddComponent<ResultDividedPart>();
            _resPart.Init(Top.gameObject);

            var leftPart = CreatePart(0f, GrowingPartWidth, worldPoint.magnitude);
            _leftPart = leftPart.gameObject.AddComponent<LeftDividedPart>();

            IsDivided = true;

            Destroy(_growing.gameObject);
        }

        public bool Check(Vector2 pointToCheck, out Vector2 collisionPoint)
        {
            var dot = Vector2.Dot(Top.transform.position.normalized, pointToCheck.normalized);
            if (_growing.Length > pointToCheck.magnitude && 1f - dot < DeadArea)
            {
                collisionPoint = pointToCheck;
                return true;
            }

            collisionPoint = Vector2.zero;
            return false;
        }

        private void Grow(float length)
        {
            _growing.Grow(length);
            Top.SetOffset(length);
        }

        private GrowingBase CreatePart(float divisionPos, float width, float length)
        {
            var part = Instantiate(GrowingPrefab, transform, false);

            part.transform.localPosition = Vector3.up * divisionPos;
            part.ChangeWidth(width);
            part.ChangeHeight(length);

            return part;
        }
    }
}