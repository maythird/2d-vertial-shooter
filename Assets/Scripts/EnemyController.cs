using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] sprites;
    public int health;
    public float speed;
    public string name;
    public int enemyScore;
    public Vector3 dir;

    private bool isDead = false;
    private float _fireTimer = 0;
    private float fireRate = 2f;
    public GameObject bulletObjA;
    public GameObject bulletObj;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        fire();
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
    }

    void OnHit(int damage)
    {
        if (isDead) return;

        health -= damage;
        sr.sprite = sprites[1];
        Invoke("ReturnDefalutSprite", 0.1f);

        if (health <= 0)
        {
            isDead = true;
            Player.Instance.score += enemyScore;
            ItemManager.Instance.DropItem(transform.position, Player.Instance.power);
            Destroy(gameObject);
        }
    }

    void ReturnDefalutSprite()
    {
        sr.sprite = sprites[0];
    }

    void fire()
    {
        _fireTimer += Time.deltaTime;
        Debug.Log($"enemy timer{_fireTimer}");
        if (_fireTimer >= fireRate)
        {
            if (name == "C")
            {
                GameObject firePoint = transform.Find("FirePoint").gameObject;
                Vector3 bullet1Pos = firePoint.transform.position;
                bullet1Pos.x = bullet1Pos.x + 0.4f;
                Vector3 bullet2Pos = firePoint.transform.position;
                bullet2Pos.x = bullet2Pos.x - 0.4f;
                GameObject bullet1 = Instantiate(bulletObjA, bullet1Pos, Quaternion.identity);
                GameObject bullet2 = Instantiate(bulletObjA, bullet2Pos, Quaternion.identity);


                _fireTimer = 0;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌");
        if (other.gameObject.tag == "Bullets")
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            Player playerScript = other.GetComponent<Player>();
            playerScript.TakeDamage(1);
        }
    }
}