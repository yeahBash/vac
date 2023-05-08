using System;
using System.Linq;
using Branch;
using Core;
using Destroyer;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameManagement
{
    public class LevelLoader : MonoBehaviour
    {
        private const float RESULT_MULTIPLIER = 10f; // TODO: calculate coefficient

        public LevelBase TestLevelToLoad;
        public CoreBase CorePrefab;
        public DestroyerBase DestroyerPrefab;

        public int Score { get; private set; } //TODO: move
        public LevelBase CurrentLevel { get; private set; }

        private void Awake()
        {
            GameManager.Instance.InitLevelLoader(this);
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (TestLevelToLoad != null) Load(TestLevelToLoad);
        }

        public void Load(LevelBase levelToLoad)
        {
            ResetLevel();

            var destroyers = levelToLoad.DestroyerPositions.Select(destroyerPos =>
                Instantiate(DestroyerPrefab, destroyerPos, Quaternion.identity));

            var core = Instantiate(CorePrefab, levelToLoad.CorePosition, Quaternion.identity);
            core.RotationSpeed = levelToLoad.RotationSpeed;
            core.PlaceBranches(levelToLoad.Branches);
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

            const int maxBranches = 10; // TODO: find a better place
            var destroyerPos = randomLevel.DestroyerPositions.First(); //TODO: change

            var branchesCount = Mathf.Clamp((int)(Random.value * maxBranches), 1, maxBranches);
            var branchesParameters = new BranchBaseParameters[branchesCount];
            for (var i = 0; i < branchesParameters.Length; i++)
                branchesParameters[i] = new BranchBaseParameters
                {
                    Length = Mathf.Clamp(Random.value * destroyerPos.magnitude, CorePrefab.Size.magnitude / 2f,
                        destroyerPos.magnitude),
                    AnglePosition = 360f / branchesCount * i
                };

            randomLevel.Branches = branchesParameters;

            return randomLevel;
        }
    }
}