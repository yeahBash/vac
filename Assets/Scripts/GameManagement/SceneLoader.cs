using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public string EnvironmentSceneName;
        public string BackgroundSceneName;

        private void Start()
        {
            LoadScene(EnvironmentSceneName, LoadSceneMode.Additive);
            LoadScene(BackgroundSceneName, LoadSceneMode.Additive);
        }

        private void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (!string.IsNullOrEmpty(sceneName))
                SceneManager.LoadScene(sceneName, loadSceneMode);
        }
    }
}