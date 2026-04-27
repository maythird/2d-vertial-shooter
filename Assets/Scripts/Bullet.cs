using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    public enum Type { None, Player, Enemy }
    public Type type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (type == Type.Enemy && other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage(damage);
            PoolManager.Instance.Release(gameObject);
        }
    }
}
