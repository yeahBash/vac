using System;
using System.Linq;
using Branch;
using Core;
using Destroyer;
using Effects;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace GameManagement
{
    public class LevelLoader : MonoBehaviour
    {
        private const float RESULT_MULTIPLIER = 10f; // TODO: calculate coefficient
        public const float MAX_BRANCH_LENGTH_COEFFICIENT = 0.9f; // TODO: get from level settings

        public LevelBase TestLevelToLoad;
        public CoreBase CorePrefab;
        public DestroyerBase DestroyerPrefab;
        public TouchEffectBase TouchEffectPrefab; //TODO: move
        
        //TODO: move
        private int _score;
        private int Score
        {
            get => _score;
            set
            {
                OnScoreDeltaChanged?.Invoke(value - _score);
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }

        public event Action<int> OnScoreChanged;
        public event Action<int> OnScoreDeltaChanged;
        public event Action OnLevelReset;

        public LevelBase CurrentLevel { get; private set; }

        private void Awake()
        {
            GameManager.Instance.InitLevelLoader(this);
            DontDestroyOnLoad(gameObject);
        }

        public void Load(LevelBase levelToLoad)
        {
            ResetLevel();

            var destroyers = levelToLoad.DestroyerPositions.Select(destroyerPos =>
                Instantiate(DestroyerPrefab, destroyerPos, Quaternion.identity));

            var core = Instantiate(CorePrefab, levelToLoad.CorePosition, Quaternion.identity);
            core.RotationSpeed = levelToLoad.RotationSpeed;
            var destroyer = destroyers.First(); //TODO: change
            core.Init(levelToLoad.Branches, destroyer, true, false);

            Instantiate(TouchEffectPrefab);

            CurrentLevel = levelToLoad;
        }

        private void ResetLevel()
        {
            var existingCameraController = GameManager.Instance.CameraController;
            if (existingCameraController != null) existingCameraController.ResetCamera();

            Score = 0;
            OnLevelReset?.Invoke();
        }

        //TODO: move
        public void AddScore(float result)
        {
            AddScore((int)Math.Round(result * RESULT_MULTIPLIER));
        }

        public void AddScore(int result)
        {
            Score += result;
        }

        public void Restart(bool isRandom)
        {
            DestroyLevel();
            Load(isRandom ? GetRandomLevel() : TestLevelToLoad);

            var background = GameManager.Instance.Background;
            if (background != null) background.Set();
        }

        private void DestroyLevel()
        {
            foreach (var levelObject in SceneManager.GetActiveScene().GetRootGameObjects())
                Destroy(levelObject);

            CurrentLevel = null;
        }

        //TODO: too simple randomizer
        private LevelBase GetRandomLevel()
        {
            var randomLevel = new LevelBase();
            randomLevel.CorePosition = TestLevelToLoad.CorePosition;
            randomLevel.DestroyerPositions = TestLevelToLoad.DestroyerPositions;
            randomLevel.RotationSpeed = TestLevelToLoad.RotationSpeed;

            var destroyerPos = randomLevel.DestroyerPositions.First(); //TODO: change
            const int minBranchesCount = 1; // TODO: find a better place
            const int maxBranchesCount = 10; // TODO: find a better place

            randomLevel.Branches = GetRandomBranchParameters(CorePrefab.Radius,
                destroyerPos.magnitude * MAX_BRANCH_LENGTH_COEFFICIENT, minBranchesCount, maxBranchesCount);

            return randomLevel;
        }

        public static BranchBaseParameters[] GetRandomBranchParameters(float minLength, float maxLength,
            int minBranchesCount, int maxBranchesCount)
        {
            var branchesCount = MathHelper.GetRandomValue(minBranchesCount, maxBranchesCount);
            var branchesParameters = new BranchBaseParameters[branchesCount];
            for (var i = 0; i < branchesParameters.Length; i++)
                branchesParameters[i] = new BranchBaseParameters
                {
                    Length = MathHelper.GetRandomValue(minLength, maxLength),
                    AnglePosition = 360f / branchesCount * i
                };

            return branchesParameters;
        }
    }
}