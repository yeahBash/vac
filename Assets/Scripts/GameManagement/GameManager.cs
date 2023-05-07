using Camera;
using UI;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public LevelLoader LevelLoader { get; private set; }
        public CameraController CameraController { get; private set; }
        public CanvasController CanvasController { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void InitLevelLoader(LevelLoader levelLoader)
        {
            LevelLoader = levelLoader;
        }

        public void InitCameraController(CameraController cameraController)
        {
            CameraController = cameraController;
        }

        public void InitCanvasController(CanvasController canvasController)
        {
            CanvasController = canvasController;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
