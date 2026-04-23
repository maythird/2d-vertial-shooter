using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    
    public GameObject[] items;

    private void Awake()
    {
        Instance = this;
    }


    public void CreateItem(Vector3 pos)
    {
        //아이템 생성
        var prefab = items[Random.Range(0, items.Length)];
        var go = Instantiate(prefab, pos, Quaternion.identity);
        var item = go.GetComponent<ItemTest>();
        StartCoroutine(item.Move());
    }
}
