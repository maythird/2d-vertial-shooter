using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    public enum ItemType
    {
        None = -1,
        Coin,
        Boom,
        Power
    }

    public ItemType itemType = ItemType.None;

    public float speed = 1f;

    public IEnumerator Move()
    {
        while (true)
        {
            if (this == null)
            {
                yield break;
            }
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= -5.5f)
                break;
            
            yield return null;
        }

        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    { 
        Destroy(gameObject);
    }
}