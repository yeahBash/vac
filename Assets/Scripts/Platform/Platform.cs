using TMPro;
using UnityEngine;
using Vac.Branch;

namespace Vac.Platform
{
    public class Platform : MonoBehaviour
    {
        private const float MAX_START_FLOWER_SIZE = 1f;
        private float _score;
        public GameObject Flower;
        public int FlowerCount;
        private TextMeshProUGUI _scoreText;
        public float Speed = 1f;
        public bool IsRotateOn = true;
        public Level.Level Level;

        public float Score
        {
            get => _score;
            set
            {
                _score = value;
                //_scoreText.text = _score.ToString("F1");
            }
        }

        private void Start()
        {
            PlaceBranches(FlowerCount, true);
        }

        private void Update()
        {
            if (IsRotateOn)
                transform.Rotate(Speed * Vector3.forward * Time.deltaTime);
        }

        private void PlaceBranch(float angle, float size)
        {
            var flower = Instantiate(Flower, transform.position, Quaternion.Euler(Vector3.forward * angle));
            flower.transform.SetParent(transform);
            flower.GetComponentInChildren<Body>().Size = size;
        }

        private void PlaceBranches(int count, bool isSizeRandom)
        {
            for (var i = 0; i < count; i++)
                PlaceBranch(360f / count * i, isSizeRandom ? Random.value * MAX_START_FLOWER_SIZE : 0f);
        }

        public void Restart(bool isRandom)
        {
            for (var i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
            PlaceBranches(isRandom ? (int) (Random.value * 10f) : 5, true);
            Score = 0f;
        }
    }
}