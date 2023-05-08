using UnityEngine;

namespace Destroyer
{
    public abstract class DestroyerBase : MonoBehaviour
    {
        public float DeadArea = 0.1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, DeadArea);
        }
    }
}