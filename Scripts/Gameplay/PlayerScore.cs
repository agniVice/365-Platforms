using System.Collections;
using UnityEngine;

public class PlayerScore : MonoBehaviour, ISubscriber, IInitializable
{
    public static PlayerScore Instance;

    private const string BestScoresKey = "BestScores";

    public int Level { get; private set; }
    public int Score { get; private set; }

    private bool _isInitialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Save;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished += Save;
    }
    public void Initialize()
    {
        _isInitialized = true;
    }
    public void AddScore(int count)
    {
        if(GameState.Instance.CurrentState == GameState.State.InGame)
        {
            Score += count;
            GameState.Instance.ScoreAdded?.Invoke();
        }
    }
    public void AddCoin()
    {
        if (GameState.Instance.CurrentState == GameState.State.InGame)
        {
            Score++;
            GameState.Instance.ScoreAdded?.Invoke();
        }
    }
    public void AddLevel()
    {
        if (GameState.Instance.CurrentState == GameState.State.InGame)
            Level++;
    }
    private void Save()
    {
        if (Score > PlayerPrefs.GetInt("Score1"))
        {
            PlayerPrefs.SetInt("Score3", PlayerPrefs.GetInt("Score2"));
            PlayerPrefs.SetInt("Score2", PlayerPrefs.GetInt("Score1"));
            PlayerPrefs.SetInt("Score1", Score);
        }
        else if (Score > PlayerPrefs.GetInt("Score2"))
        {
            PlayerPrefs.SetInt("Score3", PlayerPrefs.GetInt("Score2"));
            PlayerPrefs.SetInt("Score2", Score);
        }
        else if (Score > PlayerPrefs.GetInt("Score3"))
        {
            PlayerPrefs.SetInt("Score3", Score);
        }
    }
}