using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, Boom, Power }

    public ItemType itemType;
    public float speed = 1f;

    // 풀에서 꺼낼 때마다 하강 코루틴을 시작합니다.
    // SetActive(false) 시 자동으로 중단되므로 별도 StopCoroutine 불필요합니다.
    void OnEnable()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= -5.5f) break;
            yield return null;
        }
        ReturnToPool();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;

        Player player = other.gameObject.GetComponent<Player>();

        switch (itemType)
        {
            case ItemType.Coin:
                player.score += 1000;
                ReturnToPool();
                break;

            case ItemType.Power:
                if (player.power < 3)
                {
                    player.score += 500;
                    player.power++;
                }
                ReturnToPool();
                break;

            case ItemType.Boom:
                if (player.boomSlot < 3)
                {
                    player.score += 500;
                    player.boomSlot++;
                    ReturnToPool();
                }
                break;
        }
    }

    void ReturnToPool()
    {
        PoolManager.Instance.Release(gameObject);
    }
}
