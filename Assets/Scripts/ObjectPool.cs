using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 특정 프리팹 한 종류를 관리하는 풀. PoolManager 내부에서만 사용합니다.
/// </summary>
public class ObjectPool
{
    readonly GameObject _prefab;
    readonly Transform _parent;
    readonly Queue<GameObject> _inactive = new();

    public int CountInactive => _inactive.Count;

    public ObjectPool(GameObject prefab, Transform parent, int warmUp = 0)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < warmUp; i++)
            Preload();
    }

    void Preload()
    {
        var obj = Object.Instantiate(_prefab, _parent);
        obj.SetActive(false);
        _inactive.Enqueue(obj);
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼냅니다. 재고가 없으면 새로 생성합니다.
    /// </summary>
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _inactive.Count > 0
            ? _inactive.Dequeue()
            : Object.Instantiate(_prefab, _parent);

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 오브젝트를 비활성화해 풀에 반환합니다.
    /// </summary>
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        _inactive.Enqueue(obj);
    }
}
