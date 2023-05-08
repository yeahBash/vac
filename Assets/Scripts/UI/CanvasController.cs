using GameManagement;
using UI.Screens;
using UnityEngine;
using Screen = UI.Screens.Screen;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public LevelUI LevelUIPrefab;
        public Screen CurrentScreen { get; private set; }

        private void Awake()
        {
            if (FindObjectsOfType<CanvasController>().Length > 1)
            {
                Debug.LogError("There are other canvas controllers");
                return;
            }

            GameManager.Instance.InitCanvasController(this);
        }

        public void ToLevelUI()
        {
            ResetUI();
            CurrentScreen = Instantiate(LevelUIPrefab, transform);
        }

        private void ResetUI()
        {
            if (CurrentScreen != null)
            {
                Destroy(CurrentScreen.gameObject);
                CurrentScreen = null;
            }
        }
    }
}