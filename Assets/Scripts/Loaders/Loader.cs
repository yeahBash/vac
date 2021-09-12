using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vac
{
    public class Loader : MonoBehaviour
    {
        public int EnvironmentSceneIndex;
        
        private void Start()
        {
            SceneManager.LoadScene(EnvironmentSceneIndex, LoadSceneMode.Additive);
        }
    }
}