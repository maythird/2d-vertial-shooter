using UnityEngine;

public class BulletController : MonoBehaviour
{
    public enum Type { None, Player, Enemy }
    public Type type;

    private Vector3 direction;

    // ObjectPool.Get() 은 위치를 먼저 설정한 뒤 SetActive(true)를 호출하므로
    // OnEnable 시점에 transform.position이 이미 확정되어 있습니다.
    void OnEnable()
    {
        if (type == Type.Enemy && Player.Instance != null)
            direction = (Player.Instance.transform.position - transform.position).normalized;
    }

    void Update()
    {
        if (type == Type.Enemy)
            transform.Translate(direction * 5f * Time.deltaTime, Space.World);
        else if (type == Type.Player)
            transform.Translate(0f, 5f * Time.deltaTime, 0f);

        if (IsOutOfBounds())
            ReturnToPool();
    }

    bool IsOutOfBounds()
    {
        Vector3 p = transform.position;
        return p.y < -5f || p.y > 5f || p.x < -4f || p.x > 4f;
    }

    void ReturnToPool()
    {
        PoolManager.Instance.Release(gameObject);
    }
}
