using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CutTheFlowers
{
    public class Stem : MonoBehaviour
    {
        private SpriteRenderer _thisSpriteRenderer;
        public GameObject Blossom;
        public float GrowSpeed = 1f;

        private void Start()
        {
            _thisSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Grow(GrowSpeed);
            }
        }

        public void Divide(Vector2 worldPoint)
        {
            CreateUpperPart(worldPoint);
            CreateDownPart(worldPoint);

            Destroy(gameObject);
        }

        private void Grow(float speed)
        {
            var delta = speed * Vector3.up * Time.deltaTime;
            transform.localScale += delta;
            transform.localPosition += delta / 2;
            Blossom.transform.localPosition += delta;
        }

        private GameObject CreateUpperPart(Vector2 worldPoint)
        {
            var upperPart = new GameObject("Upper", typeof(SpriteRenderer));

            var upperSpriteRenderer = upperPart.GetComponent<SpriteRenderer>();
            SetSpriteRenderer(ref upperSpriteRenderer);

            var upBorderY = transform.position.y + transform.localScale.y / 2;
            var center = worldPoint.y + (upBorderY - worldPoint.y) / 2;
            upperPart.transform.position = new Vector3(transform.position.x, center);
            upperPart.transform.localScale = new Vector3(transform.localScale.x, (upBorderY - worldPoint.y));
            upperPart.transform.rotation = transform.rotation;
            upperPart.transform.SetParent(transform.parent);

            return upperPart;
        }

        private GameObject CreateDownPart(Vector2 worldPoint)
        {
            var downPart = new GameObject("Down", typeof(SpriteRenderer));

            var downSpriteRenderer = downPart.GetComponent<SpriteRenderer>();
            SetSpriteRenderer(ref downSpriteRenderer);

            var downBorderY = transform.position.y - transform.localScale.y / 2;
            var center = worldPoint.y - (worldPoint.y - downBorderY) / 2;
            downPart.transform.position = new Vector3(transform.position.x, center);
            downPart.transform.localScale = new Vector3(transform.localScale.x, (worldPoint.y - downBorderY));
            downPart.transform.rotation = transform.rotation;
            downPart.transform.SetParent(transform.parent);

            return downPart;
        }

        private void SetSpriteRenderer(ref SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = _thisSpriteRenderer.color;
            spriteRenderer.sprite = _thisSpriteRenderer.sprite;
        }
    }
}
