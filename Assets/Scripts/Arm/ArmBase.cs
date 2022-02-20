using System;
using UnityEngine;

namespace Arm
{
    public class ArmBase : MonoBehaviour
    {
        public GameObject Top;
        public float GrowSpeed = 1f;
        public GameObject Base;
        private float _anglePosition;
        private bool _isDivided;

        private Vector3 _originalScale;
        private Vector3 _originalTopPosition;

        private float _size;
        private Collider2D _thisCollider;
        private SpriteRenderer _thisSpriteRenderer;

        public float AnglePosition
        {
            get => _anglePosition;
            set
            {
                _anglePosition = value;
                transform.localRotation = Quaternion.Euler(Vector3.back * _anglePosition);
            }
        }

        public float Size
        {
            get => _size;
            set
            {
                _size = value;
                Grow(_size);
            }
        }

        private void Awake()
        {
            _thisCollider = GetComponent<Collider2D>();
            _thisSpriteRenderer = Base.GetComponent<SpriteRenderer>();

            _originalScale = Base.transform.localScale;
            _originalTopPosition = Top.transform.localPosition;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !_isDivided) Size += GrowSpeed * Time.deltaTime;
        }

        public void Divide(Vector2 worldPoint)
        {
            var dividePoint = Base.transform.InverseTransformPoint(worldPoint);
            var blossomPos = Base.transform.InverseTransformPoint(Top.transform.position);
            var res = blossomPos.y - dividePoint.y;

            CreatePart(dividePoint, Vector3.zero, new Vector3(1f, res, 1f));
            CreatePart(dividePoint, Vector3.forward * 180f, new Vector3(1f, dividePoint.y, 1f));

            _isDivided = true;

            Destroy(Base);
            Destroy(_thisCollider);

            Destroy(gameObject);
        }

        private void Grow(float size)
        {
            Base.transform.localScale = _originalScale + size * Vector3.up;
            Top.transform.localPosition = _originalTopPosition + size * Vector3.up;
        }

        private GameObject CreatePart(Vector2 pos, Vector3 eulerRot, Vector3 scale)
        {
            var part = new GameObject("Part", typeof(SpriteRenderer));

            var spriteRenderer = part.GetComponent<SpriteRenderer>();
            SetSpriteRenderer(spriteRenderer);

            part.transform.SetParent(transform, false);
            part.transform.localPosition = Vector3.up * pos.y;
            part.transform.localRotation = Quaternion.Euler(eulerRot);
            part.transform.localScale = scale;

            return part;
        }

        private void SetSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            spriteRenderer.sprite = _thisSpriteRenderer.sprite;
            spriteRenderer.color = _thisSpriteRenderer.color;
        }
    }

    [Serializable]
    public struct ArmParameters
    {
        public float Size;
        public float AnglePosition;
    }
}