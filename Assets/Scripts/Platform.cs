using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CutTheFlowers
{
    public class Platform : MonoBehaviour
    {
        public float Speed = 1f;

        private void Update()
        {
            transform.Rotate(Speed * Vector3.forward * Time.deltaTime);
        }
    }
}