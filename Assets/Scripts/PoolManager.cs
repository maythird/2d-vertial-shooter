using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀 싱글톤. Inspector에서 프리팹과 WarmUp 수를 등록하고,
/// Get / Release 로 풀을 사용합니다.
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public struct PoolEntry
    {
        public GameObject prefab;
        [Min(0)] public int warmUp;
    }

    [SerializeField] PoolEntry[] entries;

    // 프리팹 → 풀
    readonly Dictionary<GameObject, ObjectPool> _pools = new();
    // 인스턴스 → 원본 프리팹 (Release 시 어느 풀로 돌아갈지 추적)
    readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var e in entries)
            RegisterPool(e.prefab, e.warmUp);
    }

    void RegisterPool(GameObject prefab, int warmUp)
    {
        if (prefab == null || _pools.ContainsKey(prefab)) return;
        _pools[prefab] = new ObjectPool(prefab, transform, warmUp);
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼냅니다.
    /// Inspector에 등록되지 않은 프리팹은 자동으로 풀이 생성됩니다.
    /// </summary>
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(prefab))
            RegisterPool(prefab, 0);

        GameObject obj = _pools[prefab].Get(position, rotation);
        _instanceToPrefab[obj] = prefab;
        return obj;
    }

    /// <summary>
    /// 오브젝트를 풀에 반환합니다.
    /// 풀에 등록되지 않은 오브젝트는 Destroy로 처리합니다.
    /// </summary>
    public void Release(GameObject obj)
    {
        if (obj == null) return;

        if (_instanceToPrefab.TryGetValue(obj, out GameObject prefab))
            _pools[prefab].Release(obj);
        else
            Destroy(obj);
    }
}
