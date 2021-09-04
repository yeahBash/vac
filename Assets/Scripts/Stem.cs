using UnityEngine;

namespace CutTheFlowers
{
    public class Stem : MonoBehaviour
    {
        private bool _isDivided;
        private Collider2D _thisCollider;
        private SpriteRenderer _thisSpriteRenderer;
        public GameObject Blossom;
        public float GrowSpeed = 1f;
        public GameObject SpriteObj;

        private void Start()
        {
            _thisCollider = GetComponent<Collider2D>();
            _thisSpriteRenderer = SpriteObj.GetComponent<SpriteRenderer>();
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
            var blossomPos = SpriteObj.transform.InverseTransformPoint(Blossom.transform.position);

            CreatePart(dividePoint, Vector3.zero, new Vector3(1f, blossomPos.y - dividePoint.y, 1f));
            CreatePart(dividePoint, Vector3.forward * 180f, new Vector3(1f, dividePoint.y, 1f));

            _isDivided = true;

            Destroy(SpriteObj);
            Destroy(_thisCollider);
        }

        private void Grow(Vector3 delta)
        {
            transform.localScale += delta;
            Blossom.transform.localPosition += delta;
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
}