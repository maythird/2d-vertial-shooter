using UnityEngine;
using System.Collections.Generic;
public class ObjectPoolMain : MonoBehaviour
{
    public GameObject playerBullets;
    public GameObject playerSmallBullets;

    List<GameObject> Bullets0List = new List<GameObject>();
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(playerBullets);
            go.SetActive(false);
        }
    }

    private void Update()
    {
        GetPlayerBullets();
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
    
    public GameObject GetPlayerBullets()
    {
        for (int i = 0; i < Bullets0List.Count; i++)
        {
            GameObject bullet0 = Bullets0List[i];
            if (bullet0.activeInHierarchy == false)
            {
                return bullet0; 
            }
            
            
        }
        return null;
    }
}
