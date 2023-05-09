using UnityEngine;

namespace Background
{
    public class BackgroundMarker : MonoBehaviour
    {
        private BackgroundFadePanel _fadePanel;

        public void SetFadePanel(float radius)
        {
            // shouldn't change instructions order
            var coreRenderers = GetComponentsInChildren<SpriteRenderer>();
            var resource = Resources.Load<BackgroundFadePanel>("Background/FadePanel");
            _fadePanel = Instantiate(resource, transform);
            _fadePanel.Init(radius, transform.localScale.y); // get only x because it must be equal to y

            foreach (var coreRenderer in coreRenderers)
                coreRenderer.sortingOrder = _fadePanel.SortingOrder - 1;
        }

        private void OnDestroy()
        {
            Destroy(_fadePanel.gameObject);
        }
    }
}
