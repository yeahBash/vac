using UnityEngine;

namespace CutTheFlowers
{
    public class Platform : MonoBehaviour
    {

        public GameObject Flower;
        public float Speed = 1f;
        
        private void Start()
        {

        }

        private void Update()
        {
            transform.Rotate(Speed * Vector3.forward * Time.deltaTime);
        }

    }
}