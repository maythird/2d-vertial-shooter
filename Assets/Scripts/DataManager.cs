using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public StageData StageData { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadStageData();
    }

    void LoadStageData()
    {
        TextAsset json = Resources.Load<TextAsset>("stage_data");
        if (json == null)
        {
            Debug.LogError("[DataManager] stage_data.json을 Resources 폴더에서 찾을 수 없습니다.");
            return;
        }

        StageData = JsonUtility.FromJson<StageData>(json.text);
        Debug.Log($"[DataManager] stage_data 로드 완료 — {StageData.waves.Length}웨이브");
    }
}
