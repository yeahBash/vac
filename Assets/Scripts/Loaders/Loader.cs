using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loaders
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