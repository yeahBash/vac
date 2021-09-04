using UnityEngine;

namespace CutTheFlowers
{
    public class Blade : MonoBehaviour
    {
        private readonly RaycastHit2D[] _raycastHits = new RaycastHit2D[1];
        private Collider2D _thisCollider;
        
        private void Start()
        {
            _thisCollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            var raycastResultsNumber = _thisCollider.Raycast(transform.right, _raycastHits,
                _thisCollider.bounds.extents.x);

            if (raycastResultsNumber > 0)
            {
                var raycastHit = _raycastHits[0];
                var hitObj = raycastHit.transform.gameObject;
                if (hitObj.CompareTag("Stem")) hitObj.GetComponent<Stem>().Divide(raycastHit.point);
            }
        }
    }
}