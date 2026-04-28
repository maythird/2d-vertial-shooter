using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyGenerator : MonoBehaviour
{
    [FormerlySerializedAs("Enemys")]
    public GameObject[] Enemies;
    public Transform[] SpawnPoints;
    public Transform[] DestinationPoints;

    [SerializeField] private float waveCooldown = 3f;

    private StageData stageData;

    public int CurrentWave { get; private set; } = 0;
    public int TotalWaves  { get; private set; } = 0;

    void OnEnable()
    {
        GameManager.OnRestart += ResetStage;
    }

    void OnDisable()
    {
        GameManager.OnRestart -= ResetStage;
    }

    // DataManager.Awake()는 모든 Start() 이전에 실행되므로 여기서 안전하게 참조
    void Start()
    {
        stageData = DataManager.Instance.StageData;

        if (stageData == null)
        {
            Debug.LogError("[EnemyGenerator] DataManager에서 StageData를 받지 못했습니다.");
            return;
        }

        TotalWaves = stageData.waves.Length;
        StartCoroutine(StageRoutine());
    }

    void ResetStage()
    {
        StopAllCoroutines();
        CurrentWave = 0;
        StartCoroutine(StageRoutine());
    }

    // ──────────────────────────────────────────────
    //  스테이지 전체 흐름
    // ──────────────────────────────────────────────
    IEnumerator StageRoutine()
    {
        for (int i = 0; i < stageData.waves.Length; i++)
        {
            CurrentWave = i + 1;
            Debug.Log($"[Wave {CurrentWave}/{TotalWaves}] 시작");

            yield return StartCoroutine(WaveRoutine(stageData.waves[i]));

            if (i < stageData.waves.Length - 1)
            {
                Debug.Log($"[Wave {CurrentWave}/{TotalWaves}] 클리어 — {waveCooldown}초 후 다음 웨이브");
                yield return WaitWhilePaused(waveCooldown);
            }
        }

        Debug.Log("모든 웨이브 클리어!");
    }

    // ──────────────────────────────────────────────
    //  한 웨이브 내 순차 스폰
    // ──────────────────────────────────────────────
    IEnumerator WaveRoutine(WaveData wave)
    {
        foreach (SpawnData data in wave.enemies)
        {
            yield return WaitWhilePaused(data.delay);

            if (Player.Instance != null)
                SpawnEnemy(data);
        }
    }

    // Playing 상태가 아니면 멈추고, 재개되면 남은 시간만큼 더 기다리는 헬퍼
    IEnumerator WaitWhilePaused(float seconds)
    {
        float remaining = seconds;
        while (remaining > 0f)
        {
            if (GameManager.Instance.State == GameState.Playing)
                remaining -= Time.deltaTime;

            yield return null;
        }
    }

    // ──────────────────────────────────────────────
    //  개별 적기 스폰
    // ──────────────────────────────────────────────
    void SpawnEnemy(SpawnData data)
    {
        int enemyIndex = (int)data.enemyType;
        if (enemyIndex < 0 || enemyIndex >= Enemies.Length)
        {
            Debug.LogWarning($"enemyType {data.enemyType} 에 해당하는 프리팹이 없습니다.");
            return;
        }

        int spawnPointIndex = data.point;
        if (spawnPointIndex < 0 || spawnPointIndex >= SpawnPoints.Length)
        {
            Debug.LogWarning($"SpawnPoint 인덱스 {spawnPointIndex} 가 범위를 벗어났습니다.");
            return;
        }

        GameObject enemy = PoolManager.Instance.Get(
            Enemies[enemyIndex],
            SpawnPoints[spawnPointIndex].position,
            SpawnPoints[spawnPointIndex].rotation
        );

        Rigidbody2D rigid       = enemy.GetComponent<Rigidbody2D>();
        EnemyController logic   = enemy.GetComponent<EnemyController>();

        if (spawnPointIndex == 5 || spawnPointIndex == 6)
        {
            int dest = Random.Range(2, 4);
            ApplyDiagonalMovement(enemy, logic, spawnPointIndex, dest);
        }
        else if (spawnPointIndex == 7 || spawnPointIndex == 8)
        {
            int dest = Random.Range(0, 2);
            ApplyDiagonalMovement(enemy, logic, spawnPointIndex, dest);
        }
        else
        {
            logic.dir = Vector3.zero;
            rigid.linearVelocity = new Vector2(0, logic.speed * -1);
        }
    }

    void ApplyDiagonalMovement(GameObject enemy, EnemyController logic, int spawnIdx, int destIdx)
    {
        Vector3 dir = DestinationPoints[destIdx].position - SpawnPoints[spawnIdx].position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        logic.dir = dir.normalized;
        DrawArrow.ForDebug2D(enemy.transform.position, dir, 2f);
    }
}
