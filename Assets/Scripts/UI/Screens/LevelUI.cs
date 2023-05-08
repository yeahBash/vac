using GameManagement;
using TMPro;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LevelUI : Screen
    {
        public TextMeshProUGUI ScoreText;
        public Image ScoreIcon;

        //TODO: change (add event)
        private void Update()
        {
            ScoreText.text = GameManager.Instance.LevelLoader.Score.ToString();
        }

        public void Restart(bool isRandom)
        {
            GameManager.Instance.LevelLoader.Restart(isRandom);
        }
    }
}