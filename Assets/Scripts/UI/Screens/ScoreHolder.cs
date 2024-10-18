using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class ScoreHolder : MonoBehaviour
    {
        private int MAX_INCREMENTS_COUNT = 3;
        public TextMeshProUGUI ScoreText;
        public Image Icon;
        public VerticalLayoutGroup IncrementLayout;
        public TextMeshProUGUI ScoreIncrementTextPrefab;

        public void SetScore(int score)
        {
            ScoreText.SetText($"{score}");
        }

        public void AddIncrement(int increment)
        {
            if (IncrementLayout.transform.childCount >= MAX_INCREMENTS_COUNT)
            {
                var firstIncrement = IncrementLayout.transform.GetChild(0).gameObject;
                Destroy(firstIncrement);
            }

            var scoreText = Instantiate(ScoreIncrementTextPrefab, IncrementLayout.transform);
            scoreText.SetText($"+{increment}");
        }

        public void ResetIncrements()
        {
            for (var i = 0; i < IncrementLayout.transform.childCount; i++)
            {
                var child = IncrementLayout.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
    }
}