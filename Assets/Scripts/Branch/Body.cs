using UnityEngine;

namespace Vac.Branch
{
    public class Body : MonoBehaviour
    {
        private bool _isDivided;
        private Collider2D _thisCollider;
        private SpriteRenderer _thisSpriteRenderer;
        private Core.Core _core;
        public GameObject Top;
        public float GrowSpeed = 1f;
        public GameObject SpriteObj;
        
        public float Size
        {
            set => Grow(Vector3.up * value);
        }

        private void Start()
        {
            _thisCollider = GetComponent<Collider2D>();
            _thisSpriteRenderer = SpriteObj.GetComponent<SpriteRenderer>();
            _core = GetComponentInParent<Core.Core>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !_isDivided)
            {
                var delta = GrowSpeed * Vector3.up * Time.deltaTime;
                Grow(delta);
            }
        }

        public void Divide(Vector2 worldPoint)
        {
            var dividePoint = SpriteObj.transform.InverseTransformPoint(worldPoint);
            var blossomPos = SpriteObj.transform.InverseTransformPoint(Top.transform.position);
            var res = blossomPos.y - dividePoint.y;
            _core.Score += res;
            CreatePart(dividePoint, Vector3.zero, new Vector3(1f, blossomPos.y - dividePoint.y, 1f));
            CreatePart(dividePoint, Vector3.forward * 180f, new Vector3(1f, dividePoint.y, 1f));

            _isDivided = true;

            Destroy(SpriteObj);
            Destroy(_thisCollider);

            Destroy(transform.parent.gameObject);
        }

        private void Grow(Vector3 delta)
        {
            transform.localScale += delta;
            Top.transform.localPosition += delta;
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

    [System.Serializable]
    public struct BranchParameters
    {
        public float Size;
        public float AnglePosition;
    }
}