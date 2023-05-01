using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);
    }

    public void AddScore(int increment)
    {
        Score += increment;
    }

    private void OnDestroy()
    {
        Destroy(Instance.gameObject);
    }
}
