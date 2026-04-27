using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Coin,
        Boom,
        Power
    }

    public ItemType itemType;
    public float speed = 1f;

    public IEnumerator Move()
    {
        while (true)
        {
            if (this == null) yield break;
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= -5.5f) break;
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;

        Player player = other.gameObject.GetComponent<Player>();

        switch (itemType)
        {
            case ItemType.Coin:
                player.score += 1000;
                Destroy(gameObject);
                break;
            case ItemType.Power:
                if (player.power < 3)
                {
                    player.score += 500;
                    player.power++;
                }
                Destroy(gameObject);
                break;
            case ItemType.Boom:
                if (player.boomSlot < 3)
                {
                    player.score += 500;
                    player.boomSlot++;
                    Destroy(gameObject);
                }
                break;
        }
    }
}
