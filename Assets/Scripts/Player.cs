using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePoint;
    public GameObject BulletPrefab;
    public GameObject PlayerBulletPrefab;
    public GameObject BoomPrefab;
    private Collider2D col;
    private SpriteRenderer sr;

    Animator animator;
    public int score = 0;
    public int power = 1;
    public int life = 3;
    public int boomSlot = 1;
    private float _fireTimer = 0;
    private float fireRate = 0.2f;
    private float moveSpeed = 5f;

    public float invincibleTime = 0.5f;
    private bool isInvincible = false;
    private float invincibleEndTime;

    private Vector3 startPosition;

    public GameObject PlayerSmallBulletPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        col = GetComponent<Collider2D>();
        sr = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Clamp();
        // 👉 무적 시간 끝났는지 체크
        if (isInvincible && Time.time > invincibleEndTime)
        {
            isInvincible = false;
            col.enabled = true;

            sr.color = new Color(1, 1, 1, 1);
            // 👉 원래 위치로 이동
            transform.position = startPosition;
        }

        if (isInvincible)
        {
            return; // 👉 무적 중이면 아무 입력도 안 받음
        }
        

        if (Input.GetKey(KeyCode.Space))
        {
            _fireTimer += Time.deltaTime;

            if (_fireTimer >= fireRate)
            {
                CreateBullet();
                _fireTimer = 0f;
            }
        }

        CreateBoom();

        Move();

        Debug.Log($"현재 파워 : {power}");
    }

    void CreateBullet()
    {
        if (power == 1)
        {
            CreatePower1Bullet();
        }
        else if (power == 2)
        {
            CreatePower2Bullet();
        }
        else if (power == 3)
        {
            CreatePower3Bullet();
        }
    }

    void CreatePower1Bullet()
    {
        //이동용 부모 오브젝트 생성
        GameObject PlayerParentBulletGo = Instantiate(BulletPrefab);
        PlayerParentBulletGo.transform.position = firePoint.position;
        Debug.Log(firePoint.position);


        //자식 총알 생성
        GameObject playerBulletGo = Instantiate(PlayerSmallBulletPrefab);
        playerBulletGo.transform.SetParent(PlayerParentBulletGo.transform);
        playerBulletGo.transform.position = firePoint.position;
    }


    void CreatePower2Bullet()
    {
        //이동용 부모 오브젝트 생성
        GameObject PlayerParentBulletGo = Instantiate(BulletPrefab);
        PlayerParentBulletGo.transform.position = firePoint.position;
        Debug.Log(firePoint.position);


        //왼쪽 총알
        GameObject playerBulletLeftGo = Instantiate(PlayerSmallBulletPrefab);
        playerBulletLeftGo.transform.SetParent(PlayerParentBulletGo.transform);
        Vector3 leftPos = firePoint.position;
        leftPos.x -= 0.1f;
        playerBulletLeftGo.transform.position = leftPos;


        //오른쪽 총알
        GameObject playerBulletRightGo = Instantiate(PlayerSmallBulletPrefab);
        playerBulletRightGo.transform.SetParent(PlayerParentBulletGo.transform);
        Vector3 rightPos = firePoint.position;
        rightPos.x += 0.1f;
        playerBulletRightGo.transform.position = rightPos;
    }

    void CreatePower3Bullet()
    {
        //이동용 부모 오브젝트 생성
        GameObject PlayerParentBulletGo = Instantiate(BulletPrefab);
        PlayerParentBulletGo.transform.position = firePoint.position;
        Debug.Log(firePoint.position);

        //가운데 총알
        GameObject playerBulletGo = Instantiate(PlayerBulletPrefab);
        playerBulletGo.transform.SetParent(PlayerParentBulletGo.transform);
        Debug.Log(playerBulletGo.transform.position);
        playerBulletGo.transform.position = firePoint.position;
        Debug.Log(playerBulletGo.transform.position);

        //왼쪽 총알
        GameObject playerBulletLeftGo = Instantiate(PlayerSmallBulletPrefab);
        playerBulletLeftGo.transform.SetParent(PlayerParentBulletGo.transform);
        Vector3 leftPos = firePoint.position;
        leftPos.x -= 0.25f;
        playerBulletLeftGo.transform.position = leftPos;


        //오른쪽 총알
        GameObject playerBulletRightGo = Instantiate(PlayerSmallBulletPrefab);
        playerBulletRightGo.transform.SetParent(PlayerParentBulletGo.transform);
        Vector3 rightPos = firePoint.position;
        rightPos.x += 0.25f;
        playerBulletRightGo.transform.position = rightPos;
    }

    void CreateBoom()
    {
        if (boomSlot > 0)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                boomSlot--;
                Vector3 boomPos = firePoint.position;
                boomPos.y += 3f;
                GameObject playerBoomGo = Instantiate(BoomPrefab, boomPos, Quaternion.identity);
            }
        }
    }


    private void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(dirX, dirY, 0).normalized;
        ;
        if (dirX == -1)
        {
            animator.SetInteger(name: "dirX", (int)dirX);
        }
        else if (dirX == 1)
        {
            animator.SetInteger(name: "dirX", (int)dirX);
        }
        else
        {
            animator.SetInteger(name: "dirX", (int)dirX);
        }
        // 대각선 이동 시 단위벡터로 정규화하여 속도를 일정하게 유지

        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    private void Clamp()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -2f, 2f);
        float clampedY = Mathf.Clamp(transform.position.y, -4f, 4);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void PlayerReset()
    {
        score = 0;
        power = 1;
        boomSlot = 1;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        life -= damage;

        isInvincible = true;
        sr.color = new Color(1, 1, 1, 0);
        invincibleEndTime = Time.time + invincibleTime;

        col.enabled = false; // 👉 충돌 OFF
    }
}