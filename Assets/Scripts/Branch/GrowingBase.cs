using UnityEngine;

namespace Branch
{
    public abstract class GrowingBase : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private SpriteRenderer _spriteRenderer;

        public float Length => _spriteRenderer.size.y;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Grow(float length)
        {
            ChangeHeight(length);
        }

        public void ChangeWidth(float width)
        {
            _spriteRenderer.size = new Vector2(width, _spriteRenderer.size.y);
        }

        public void ChangeHeight(float height)
        {
            _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, height);
        }
    }
}