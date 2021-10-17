using UnityEngine;
using Vac.Branch;

namespace Vac.Destroyer
{
    public class DestroyerBase : MonoBehaviour
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
                if (hitObj.CompareTag("BranchBody")) hitObj.GetComponent<BranchBodyBase>().Divide(raycastHit.point);
            }
        }
    }
}