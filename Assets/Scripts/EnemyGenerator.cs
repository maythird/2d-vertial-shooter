using UnityEngine;
using UnityEngine.Serialization;

public class EnemyGenerator : MonoBehaviour
{
    [FormerlySerializedAs("Enemys")]
    public GameObject[] Enemies;
    public Transform[] SpawnPoints;
    public Transform[] DestinationPoints;

    private float delay;
    private float waitTime;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        waitTime += Time.deltaTime;
        if (waitTime >= delay)
        {
            if (Player.Instance != null)
                SpawnEnemy();

            delay = Random.Range(0.5f, 3f);
            waitTime = 0;
        }
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, Enemies.Length);
        int ranSpawnPoint = Random.Range(0, SpawnPoints.Length);
        GameObject enemy = Instantiate(Enemies[ranEnemy], SpawnPoints[ranSpawnPoint].position,
            SpawnPoints[ranSpawnPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyController enemyLogic = enemy.GetComponent<EnemyController>();
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