using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vac.Branch;

namespace Vac.Core
{
    public class Core : MonoBehaviour
    {
        private const float MAX_START_BRANCH_SIZE = 1f;
        private float _score;
        public GameObject Branch;
        private List<GameObject> _branches = new List<GameObject>();
        public TextMeshProUGUI ScoreText;
        public float Speed = 1f;
        public bool IsRotateOn = true;
        public Level.Level Level;

        public float Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreText.text = _score.ToString("F1");
            }
        }

        private void Start()
        {
            //PlaceBranches(BranchesCount, true);
        }

        private void Update()
        {
            if (IsRotateOn)
                transform.Rotate(Speed * Vector3.forward * Time.deltaTime);
        }

        public Body PlaceBranch(float angle, float size)
        {
            var branch = Instantiate(Branch, transform.position, Quaternion.identity);
            branch.transform.SetParent(transform);
            var body = branch.GetComponentInChildren<Body>();
            body.OriginalScale = body.transform.localScale;
            body.OriginalTopPosition = body.Top.transform.localPosition;
            body.Size = size;
            body.AnglePosition = angle;
            _branches.Add(branch);
            return body;
        }

        private void PlaceBranches(int count, bool isSizeRandom)
        {
            for (var i = 0; i < count; i++)
                PlaceBranch(360f / count * i, isSizeRandom ? Random.value * MAX_START_BRANCH_SIZE : 0f);
        }

        public void Clear()
        {
            foreach (var branch in _branches)
            {
                Destroy(branch);
            }

            _branches.Clear();
        }

        public void Restart(bool isRandom)
        {
            for (var i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
            PlaceBranches(isRandom ? (int) (Random.value * 10f) : 5, true);
            Score = 0f;
        }
    }
}