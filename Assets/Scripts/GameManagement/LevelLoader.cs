using System;
using System.Linq;
using Core;
using Destroyer;
using Level;
using UnityEngine;

namespace GameManagement
{
    public class LevelLoader : MonoBehaviour
    {
        private const float RESULT_MULTIPLIER = 10f; // TODO: calculate coefficient

        public LevelBase TestLevelToLoad;
        public CoreBase CorePrefab;
        public DestroyerBase DestroyerPrefab;

        public int Score { get; private set; } //TODO: move

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
            var destroyers = levelToLoad.DestroyerPositions.Select(destroyerPos =>
                Instantiate(DestroyerPrefab, destroyerPos, Quaternion.identity));

            var core = Instantiate(CorePrefab, levelToLoad.CorePosition, Quaternion.identity);
            core.RotationSpeed = levelToLoad.RotationSpeed;
            core.PlaceBranches(levelToLoad.Branches);
            core.Destroyer = destroyers.First(); //TODO: change
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
            //Load(isRandom ? GetRandomLevel() : TestLevelToLoad);
        }

        //private LevelBase GetRandomLevel()
        //{

        //}
    }
}