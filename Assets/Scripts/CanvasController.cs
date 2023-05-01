using System;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    private void Update()
    {
        ScoreText.text = GameManager.Instance.Score.ToString();
    }
}
