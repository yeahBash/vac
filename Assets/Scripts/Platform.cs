using TMPro;
using UnityEngine;

namespace CutTheFlowers
{
    public class Platform : MonoBehaviour
    {
        private const float MAX_START_FLOWER_SIZE = 0.2f;
        private float _score;
        public GameObject Flower;
        public int FlowerCount;
        public TextMeshProUGUI ScoreText;
        public float Speed = 1f;

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
            PlaceFlowers(FlowerCount, true);
        }

        private void Update()
        {
            transform.Rotate(Speed * Vector3.forward * Time.deltaTime);
        }

        private void PlaceFlower(float angle, float size)
        {
            var flower = Instantiate(Flower, transform.position, Quaternion.Euler(Vector3.forward * angle));
            flower.transform.SetParent(transform);
            flower.GetComponentInChildren<Stem>().Size = size;
        }

        private void PlaceFlowers(int count, bool isSizeRandom)
        {
            for (var i = 0; i < count; i++)
                PlaceFlower(360f / count * i, isSizeRandom ? Random.value * MAX_START_FLOWER_SIZE : 0f);
        }
    }
}