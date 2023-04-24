using UnityEngine;

namespace Arm
{
    public class GrowingBase : MonoBehaviour
    {
        private Vector2 _originalSize;
        private SpriteRenderer _spriteRenderer;

        public float Length => _spriteRenderer.size.y;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalSize = _spriteRenderer.size;
        }

        public void Grow(float length)
        {
            ChangeHeight(_originalSize.y + length);
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
