using GameManagement;

namespace UI.Screens
{
    public class LevelUI : Screen
    {
        public ScoreHolder ScoreHolder;

        private void Awake()
        {
            GameManager.Instance.LevelLoader.OnScoreChanged += ChangeScoreText;
            GameManager.Instance.LevelLoader.OnScoreDeltaChanged += ChangeScoreIncrementText;
            GameManager.Instance.LevelLoader.OnLevelReset += ResetScore;
        }

        private void ChangeScoreText(int score)
        {
            ScoreHolder.SetScore(score);
        }

        private void ChangeScoreIncrementText(int score)
        {
            ScoreHolder.AddIncrement(score);
        }

        private void ResetScore()
        {
            ScoreHolder.ResetIncrements();
        }

        public void Restart(bool isRandom)
        {
            GameManager.Instance.LevelLoader.Restart(isRandom);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LevelLoader.OnScoreChanged -= ChangeScoreText;
                GameManager.Instance.LevelLoader.OnScoreDeltaChanged -= ChangeScoreIncrementText;
                GameManager.Instance.LevelLoader.OnLevelReset -= ResetScore;
            }
        }
    }
}