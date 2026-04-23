using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public string type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (type == "Enermy")
        {
            if (other.gameObject.tag == "Player")
            {
                Player playerScript = other.GetComponent<Player>();
                playerScript.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}