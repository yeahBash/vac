using UnityEngine;

namespace Background
{
    public class BackgroundFadePanel : MonoBehaviour
    {
        private float _coreScale;
        private SpriteRenderer _spriteRenderer;
        public int SortingOrder => _spriteRenderer.sortingOrder;
        private Color SpriteRendererColor => _spriteRenderer.color;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_coreScale == 0f) return;

            _spriteRenderer.color = ScaleToColor(_coreScale);
        }

        public void Init(float radius, float scale)
        {
            _spriteRenderer.size = 2f * radius * Vector2.one;
            _coreScale = scale;
        }

        private Color ScaleToColor(float scale)
        {
            return new Color(SpriteRendererColor.r, SpriteRendererColor.g, SpriteRendererColor.b, ScaleToAlpha(scale));
        }

        private static float ScaleToAlpha(float scale)
        {
            return 1f - scale;
        }
    }
}