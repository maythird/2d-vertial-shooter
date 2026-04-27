using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public GameObject itemCoin;
    public GameObject itemBoom;
    public GameObject itemPower;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void DropItem(Vector3 pos, int playerPower)
    {
        int roll = Random.Range(0, 10);

        GameObject prefab;
        Item.ItemType itemType;

        if (roll < 3)
        {
            return; // 30% 아이템 없음
        }
        else if (roll < 6)
        {
            prefab = itemCoin;
            itemType = Item.ItemType.Coin;
        }
        else if (roll < 8)
        {
            if (playerPower >= 3) return;
            prefab = itemPower;
            itemType = Item.ItemType.Power;
        }
        else
        {
            prefab = itemBoom;
            itemType = Item.ItemType.Boom;
        }

        // 같은 타입 아이템이 이미 화면에 있으면 스폰하지 않습니다
        foreach (var existing in FindObjectsByType<Item>(FindObjectsSortMode.None))
        {
            if (existing.itemType == itemType) return;
        }

        // Instantiate → PoolManager.Get 으로 교체
        // Item.OnEnable()에서 Move 코루틴을 시작하므로 StartCoroutine 호출 불필요합니다
        var go = PoolManager.Instance.Get(prefab, pos, Quaternion.identity);
        go.GetComponent<Item>().itemType = itemType;
    }
}
