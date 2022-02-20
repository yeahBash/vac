using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loaders
{
    public class Loader : MonoBehaviour
    {
        public int EnvironmentSceneIndex;
        public int BackgroundSceneIndex;
        public int TestSceneIndex;

        private void Start()
        {
            SceneManager.LoadScene(EnvironmentSceneIndex, LoadSceneMode.Additive);
            SceneManager.LoadScene(BackgroundSceneIndex, LoadSceneMode.Additive);
            SceneManager.LoadScene(TestSceneIndex, LoadSceneMode.Additive);
        }
    }
}