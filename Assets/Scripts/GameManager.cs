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
        {
            if (PoolManager.Instance != null)
                PoolManager.Instance.Release(go);
            else
                Destroy(go);
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("EnemyBullets"))
        {
            if (PoolManager.Instance != null)
                PoolManager.Instance.Release(go);
            else
                Destroy(go);
        }

        // 풀링된 적은 Destroy 대신 Release로 반환합니다
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (PoolManager.Instance != null)
                PoolManager.Instance.Release(go);
            else
                Destroy(go);
        }

        // 풀링된 아이템은 Release로 반환합니다
        foreach (var item in FindObjectsByType<Item>(FindObjectsSortMode.None))
        {
            if (PoolManager.Instance != null)
                PoolManager.Instance.Release(item.gameObject);
            else
                Destroy(item.gameObject);
        }
    }
}
