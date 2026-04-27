using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public Sprite[] sprites;
    public int health;
    public float speed;
    public string name;
    public int enemyScore;
    public Vector3 dir;

    private int maxHealth;
    private bool isDead = false;
    private float _fireTimer = 0;
    private float fireRate = 2f;
    public GameObject bulletObjA;
    public GameObject bulletObj;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health; // 프리팹 초기값을 저장
    }

    // 풀에서 꺼낼 때마다 상태를 초기화합니다
    void OnEnable()
    {
        isDead = false;
        _fireTimer = 0f;
        health = maxHealth; // 체력을 초기값으로 복구

        if (sr != null && sprites != null && sprites.Length > 0)
            sr.sprite = sprites[0];

        // 이전 스폰 때의 물리 속도 초기화 (EnemyGenerator에서 다시 설정)
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        fire();

        if (transform.position.y < -5 ||
            transform.position.x < -4 ||
            transform.position.x > 4)
        {
            ReturnToPool();
        }
    }

    void OnHit(int damage)
    {
        if (isDead) return;

        health -= damage;
        sr.sprite = sprites[1];
        Invoke(nameof(ReturnDefaultSprite), 0.1f);

        if (health <= 0)
        {
            Player.Instance.score += enemyScore;
            ItemManager.Instance.DropItem(transform.position, Player.Instance.power);
            ReturnToPool();
        }
    }

    void ReturnDefaultSprite()
    {
        sr.sprite = sprites[0];
    }

    /// <summary>
    /// 적을 풀에 반환합니다. 중복 반환을 방지하기 위해 isDead 플래그를 사용합니다.
    /// </summary>
    void ReturnToPool()
    {
        if (isDead) return;
        isDead = true;
        CancelInvoke(nameof(ReturnDefaultSprite));
        PoolManager.Instance.Release(gameObject);
    }

    void fire()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= fireRate)
        {
            if (name == "C")
            {
                GameObject firePoint = transform.Find("FirePoint").gameObject;
                Vector3 bullet1Pos = firePoint.transform.position;
                bullet1Pos.x += 0.4f;
                Vector3 bullet2Pos = firePoint.transform.position;
                bullet2Pos.x -= 0.4f;
                PoolManager.Instance.Get(bulletObjA, bullet1Pos, Quaternion.identity);
                PoolManager.Instance.Get(bulletObjA, bullet2Pos, Quaternion.identity);
                _fireTimer = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullets")
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            PoolManager.Instance.Release(other.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            Player playerScript = other.GetComponent<Player>();
            playerScript.TakeDamage(1);
        }
    }
}
