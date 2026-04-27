using System;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; } = GameState.Playing;

    public static event Action OnGameOver;
    public static event Action OnRestart;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NotifyPlayerDead()
    {
        if (State == GameState.GameOver) return;

        State = GameState.GameOver;
        ClearField();
        OnGameOver?.Invoke();
    }

    public void Restart()
    {
        if (State != GameState.GameOver) return;

        State = GameState.Playing;
        OnRestart?.Invoke();
    }

    void ClearField()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Bullets"))
            Destroy(go);
        foreach (var go in GameObject.FindGameObjectsWithTag("EnemyBullets"))
            Destroy(go);
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(go);
        foreach (var item in FindObjectsByType<Item>(FindObjectsSortMode.None))
            Destroy(item.gameObject);
    }
}
