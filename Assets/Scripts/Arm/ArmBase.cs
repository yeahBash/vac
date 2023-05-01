using System;
using UnityEngine;

namespace Arm
{
    public class ArmBase : MonoBehaviour
    {
        public TopBase TopPrefab;
        public GrowingBase GrowingPrefab;
        public RectTransform ArmHolderPrefab;
        public float GrowingPartWidth = 0.1f;
        public float TopScale = 0.25f;

        public bool IsBackground;
        public float DeadArea = 0.02f;
        public float GrowSpeed = 1f;

        [HideInInspector] public TopBase Top;
        private GameObject _destroyer;
        private GrowingBase _growing;

        private float _anglePosition;
        private float _length;
        private Canvas _canvas;
        private RectTransform _dividedPartRectTransform;
        private RectTransform _targetHolder;
        [SerializeField] private float ResPartToIconDiff = 1f;
        [SerializeField] private float ResPartToIconSpeed = 4f;
        private GrowingBase _leftPart;

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

        public bool IsDivided { get; private set; }

        private void Awake()
        {
            _destroyer = GameObject.FindGameObjectWithTag("Destroyer");
        }

        private void Start()
        {
            Top = Instantiate(TopPrefab, transform, false);
            Top.Init(TopScale);

            _growing = Instantiate(GrowingPrefab, transform, false);
            _growing.ChangeWidth(GrowingPartWidth);

            _canvas = FindObjectOfType<Canvas>(); // TODO: change
        }

        private void Update()
        {
            if (IsDivided)
            {
                if (_dividedPartRectTransform != null)
                {
                    _dividedPartRectTransform.Translate(
                        (Vector3.zero - _dividedPartRectTransform.anchoredPosition3D).normalized * (ResPartToIconSpeed * Time.deltaTime));
                    if (_dividedPartRectTransform.anchoredPosition.magnitude < ResPartToIconDiff)
                        Destroy(_dividedPartRectTransform.gameObject);

                    if (_leftPart.Length >= 0)
                    {
                        _leftPart.ChangeHeight(_leftPart.Length - Time.deltaTime * ResPartToIconSpeed);
                    }
                }
                return;
            }

            if (Input.GetMouseButton(0) && !IsBackground)
                Length += GrowSpeed * Time.deltaTime;

            var isCollided = Check(out var point);
            if (isCollided)
                Divide(point);
        }

        public void Divide(Vector2 worldPoint)
        {
            var res = _growing.Length - worldPoint.magnitude;

            GameManager.Instance.AddScore((int)Math.Round(res * 10f));
            var resPart = CreatePart(worldPoint.magnitude, Quaternion.identity, res);
            Instantiate(ArmHolderPrefab, _canvas.transform);
            Top.gameObject.AddComponent<RectTransform>();
            _dividedPartRectTransform = Instantiate(ArmHolderPrefab, Top.transform.localPosition, Quaternion.identity, _canvas.transform);
            resPart.gameObject.AddComponent<RectTransform>();
            Top.transform.SetParent(_dividedPartRectTransform, true);
            resPart.transform.SetParent(_dividedPartRectTransform, true);

            _leftPart = CreatePart(worldPoint.magnitude, new Quaternion(0f, -1f, 0f, 0f), worldPoint.magnitude);
            _leftPart.transform.localPosition = Vector3.zero;
            IsDivided = true;

            Destroy(_growing.gameObject);
        }

        private bool Check(out Vector2 collisionPoint)
        {
            var dot = Vector2.Dot(Top.transform.position.normalized, _destroyer.transform.position.normalized);
            if (_growing.Length > _destroyer.transform.position.magnitude && 1f - dot < DeadArea)
            {
                collisionPoint = _destroyer.transform.position;
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

        private GrowingBase CreatePart(float divisionPos, Quaternion rot, float length)
        {
            var part = Instantiate(GrowingPrefab, transform, false);

            part.transform.localPosition = Vector3.up * divisionPos;
            part.transform.localRotation = rot;
            part.ChangeWidth(GrowingPartWidth);
            part.ChangeHeight(length);

            return part;
        }
    }

    [Serializable]
    public struct ArmParameters
    {
        public float Size;
        public float AnglePosition;
    }
}