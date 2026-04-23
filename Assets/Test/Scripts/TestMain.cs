using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TestMain : MonoBehaviour
{
    public Button btn;
    public En enemyAGo;

    void Start()
    {
        enemyAGo.Ondie = (Vector3) => { ItemManager.Instance.CreateItem(Vector3); };

        btn.onClick.AddListener(() =>
        {
            if (enemyAGo != null)
            {
                enemyAGo.TakeDamage(5);
            }
        });
    }
}