using System;
using UnityEngine;
using Vac.Core;

namespace Vac.Branch
{
    public class BranchBodyBase : MonoBehaviour
    {
        private bool _isDivided;
        private Collider2D _thisCollider;
        private SpriteRenderer _thisSpriteRenderer;
        private CoreBase _coreBase;
        private float _size;
        private float _anglePosition;
        public Vector3 OriginalScale;
        public Vector3 OriginalTopPosition;
        public GameObject Top;
        public float GrowSpeed = 1f;
        public GameObject SpriteObj;
        public float AnglePosition
        {
            get => _anglePosition;
            set
            {
                _anglePosition = value;
                transform.parent.localRotation = Quaternion.Euler(Vector3.back * _anglePosition);
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

        private void Start()
        {
            _thisCollider = GetComponent<Collider2D>();
            _thisSpriteRenderer = SpriteObj.GetComponent<SpriteRenderer>();
            _coreBase = GetComponentInParent<CoreBase>();
            OriginalScale = transform.localScale;
            OriginalTopPosition = Top.transform.localPosition;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !_isDivided)
            {
                Size += GrowSpeed * Time.deltaTime;
            }
        }

        public void Divide(Vector2 worldPoint)
        {
            var dividePoint = SpriteObj.transform.InverseTransformPoint(worldPoint);
            var blossomPos = SpriteObj.transform.InverseTransformPoint(Top.transform.position);
            var res = blossomPos.y - dividePoint.y;
            _coreBase.Score += res;
            CreatePart(dividePoint, Vector3.zero, new Vector3(1f, blossomPos.y - dividePoint.y, 1f));
            CreatePart(dividePoint, Vector3.forward * 180f, new Vector3(1f, dividePoint.y, 1f));

            _isDivided = true;

            Destroy(SpriteObj);
            Destroy(_thisCollider);

            Destroy(transform.parent.gameObject);
        }

        private void Grow(float size)
        {
            transform.localScale = OriginalScale + size * Vector3.up;
            Top.transform.localPosition = OriginalTopPosition + size * Vector3.up;
        }

        private GameObject CreatePart(Vector2 pos, Vector3 eulerRot, Vector3 scale)
        {
            var part = new GameObject("Part", typeof(SpriteRenderer));

            var spriteRenderer = part.GetComponent<SpriteRenderer>();
            SetSpriteRenderer(ref spriteRenderer);

            part.transform.SetParent(transform, false);
            part.transform.localPosition = Vector3.up * pos.y;
            part.transform.localRotation = Quaternion.Euler(eulerRot);
            part.transform.localScale = scale;

            return part;
        }

        private void SetSpriteRenderer(ref SpriteRenderer spriteRenderer)
        {
            spriteRenderer.sprite = _thisSpriteRenderer.sprite;
            spriteRenderer.color = _thisSpriteRenderer.color;
        }
    }

    [Serializable]
    public struct BranchParameters
    {
        public float Size;
        public float AnglePosition;
    }
}