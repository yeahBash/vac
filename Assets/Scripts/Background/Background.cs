using System.Linq;
using Core;
using GameManagement;
using UnityEngine;
using Utilities;

namespace Background
{
    public class Background : MonoBehaviour
    {
        public CoreBase CorePrefab;
        public int MinBranchesCount = 1;
        public int MaxBranchesCount = 10;
        public float MinBranchesLength = 1f;
        public float MaxBranchesLength = 2f;
        public float MinRotationSpeed = 80f;
        public float MaxRotationSpeed = 110f;

        private CoreBase[] _cores; // TODO: make procedural

        private void Awake()
        {
            if (FindObjectsOfType<Background>().Length > 1)
            {
                Debug.LogError("There are other backgrounds");
                return;
            }

            if (GameManager.Instance != null)
                GameManager.Instance.SetBackground(this);
            else
                Set();
        }

        private void OnDestroy()
        {
            foreach (var core in _cores.Where(c => c != null)) Destroy(core.gameObject);
        }

        // TODO: make randomizer
        public void Set()
        {
            _cores = gameObject.scene.GetRootGameObjects().Select(root => root.GetComponent<CoreBase>())
                .Where(core => core != null).ToArray();
            foreach (var core in _cores)
            {
                var fadePanel = core.GetComponentInChildren<BackgroundFadePanel>();
                if (fadePanel != null)
                    Destroy(fadePanel.gameObject);

                Init(core);
            }
        }

        private void Init(CoreBase core)
        {
            core.RotationSpeed = MathHelper.GetRandomValue(MinRotationSpeed, MaxRotationSpeed); // TODO: make procedural
            core.Init(
                LevelLoader.GetRandomBranchParameters(MinBranchesLength * LevelLoader.MAX_BRANCH_LENGTH_COEFFICIENT,
                    MaxBranchesLength * LevelLoader.MAX_BRANCH_LENGTH_COEFFICIENT, MinBranchesCount, MaxBranchesCount),
                false, true);
        }
    }
}