using UnityEngine;

public class P : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var ItemTest = other.gameObject.GetComponent<ItemTest>();
        Debug.Log($"아이템 타입 : {ItemTest.itemType}");
    }
}
