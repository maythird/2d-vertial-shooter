using UnityEngine;

public class Boom : MonoBehaviour
{
    private float timer;
    private float delayTime = 1f;

    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delayTime)
        {
            Destroy(gameObject);
        }
    }
}