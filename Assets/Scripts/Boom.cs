using UnityEngine;

public class Boom : MonoBehaviour
{
    public float duration = 2f;

    void Start()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(go);
        foreach (var go in GameObject.FindGameObjectsWithTag("EnemyBullets"))
            Destroy(go);

        Destroy(gameObject, duration);
    }
}
