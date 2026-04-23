using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class En : MonoBehaviour
{
    public int hp;
    public int maxHp = 10;

    
    public Action<Vector3> Ondie;
    
    public void Start()
    {
        hp = maxHp;
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Ondie(transform.position);
            
            Destroy(gameObject);
        }
    }

    
        
    }

