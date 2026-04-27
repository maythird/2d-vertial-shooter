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

        foreach (var existing in FindObjectsByType<Item>(FindObjectsSortMode.None))
        {
            if (existing.itemType == itemType) return;
        }

        var go = Instantiate(prefab, pos, Quaternion.identity);
        var item = go.GetComponent<Item>();
        item.itemType = itemType;
        StartCoroutine(item.Move());
    }
}
