using System;
using System.Linq;
using Branch;
using Core;
using Destroyer;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace GameManagement
{
    public class LevelLoader : MonoBehaviour
    {
        private const float RESULT_MULTIPLIER = 10f; // TODO: calculate coefficient

        public LevelBase TestLevelToLoad;
        public Background.Background TestBackground;
        public CoreBase CorePrefab;
        public DestroyerBase DestroyerPrefab;

        public int Score { get; private set; } //TODO: move
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
            core.Init(levelToLoad.Branches, true, false);
            core.Destroyer = destroyers.First(); //TODO: change

            CurrentLevel = levelToLoad;
        }

        private void ResetLevel()
        {
            var existingCameraController = GameManager.Instance.CameraController;
            if (existingCameraController != null) existingCameraController.ResetCamera();

            Score = 0;
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
            GameManager.Instance.Background.Set();
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

            randomLevel.Branches = GetRandomBranchParameters(CorePrefab.Radius, destroyerPos.magnitude, minBranchesCount, maxBranchesCount);

            return randomLevel;
        }

        public static BranchBaseParameters[] GetRandomBranchParameters(float minLength, float maxLength, int minBranchesCount, int maxBranchesCount)
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