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
        public Background.Background Background { get; private set; }

        private bool _isInited;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!_isInited)
                if (LevelLoader != null && CameraController != null && CanvasController != null)
                {
                    LevelLoader.Load(LevelLoader.TestLevelToLoad);
                    _isInited = true;
                }
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
            CanvasController.ToLevelUI(); // TODO: move when there will be more ui screens
        }

        public void SetBackground(Background.Background background)
        {
            Background = background;
            Background.Set();
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
