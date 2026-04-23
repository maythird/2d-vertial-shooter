using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] Enemys;
    public Transform[] SpawnPoints;
    public Transform[] DestinationPoints;

    private GameObject player;
    private float delay;
    private float waitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= delay)
        {
            if (player != null)
            {
                SpawnEnemy();
            }

            delay = Random.Range(0.5f, 3f);
            waitTime = 0;
        }
    }

    void SpawnEnemy()
    {
        int ranEnermy = Random.Range(0, Enemys.Length);
        int ranSpawnPoint = Random.Range(0, SpawnPoints.Length);
        GameObject enemy = Instantiate(Enemys[ranEnermy], SpawnPoints[ranSpawnPoint].position,
            SpawnPoints[ranSpawnPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyController enemyLogic = enemy.GetComponent<EnemyController>();
        enemyLogic.player = player;
        if (ranSpawnPoint == 5 || ranSpawnPoint == 6)
        {
            int ranDestinationPoint = Random.Range(2, 4);
            Debug.Log($"RD: {ranDestinationPoint}");
            Vector3 dir = DestinationPoints[ranDestinationPoint].position - SpawnPoints[ranSpawnPoint].position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
            enemyLogic.dir = dir.normalized;
            DrawArrow.ForDebug2D(enemy.transform.position, dir, 2f);
        }
        else if (ranSpawnPoint == 7 || ranSpawnPoint == 8)
        {
            int ranDestinationPoint = Random.Range(0, 2);
            Debug.Log($"RD: {ranDestinationPoint}");
            Vector3 dir = DestinationPoints[ranDestinationPoint].position - SpawnPoints[ranSpawnPoint].position;
            transform.rotation = Quaternion.Euler(dir.normalized);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
            enemyLogic.dir = dir.normalized;
            DrawArrow.ForDebug2D(enemy.transform.position, dir, 2f);
        }
        else
        {
            rigid.linearVelocity = new Vector2(0, enemyLogic.speed * (-1));
        }
    }
}