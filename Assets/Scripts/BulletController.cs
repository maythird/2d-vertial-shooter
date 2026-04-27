using UnityEngine;

public class BulletController : MonoBehaviour
{
    public enum Type
    {
        None,
        Player,
        Enemy
    }
    public Type type;
    private Vector3 direction;

    void Start()
    {
        if (Player.Instance != null)
            direction = (Player.Instance.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (type == Type.Enemy)
        {
            Debug.Log("적 미사일 발사중");
            Debug.Log(direction);
            transform.Translate(direction * 5 * Time.deltaTime, Space.World);
        }
        else if (type == Type.Player)
        {
            transform.Translate(0, 5 * Time.deltaTime, 0);
        }


        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }

        if (transform.position.x < -4)
        {
            Destroy(gameObject);
        }

        if (transform.position.x > 4)
        {
            Destroy(gameObject);
        }

        if (transform.position.y > 5)
        {
            Destroy(gameObject);
        }
    }
}