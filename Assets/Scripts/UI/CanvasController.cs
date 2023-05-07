using GameManagement;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        private Canvas _canvas;

        private void Awake()
        {
            if (FindObjectsOfType<CanvasController>().Length > 1)
            {
                Debug.LogError("There are other canvas controllers");
                return;
            }

            GameManager.Instance.InitCanvasController(this);
            _canvas = GetComponent<Canvas>();
        }

        //TODO: change (add event)
        private void Update()
        {
            ScoreText.text = GameManager.Instance.LevelLoader.Score.ToString();
        }
    }
}