using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌");
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            switch (type)
            {
                case "Coin":
                    player.score += 1000;
                    Debug.Log("코인충돌확인");
                    Destroy(gameObject);
                    break;
                case "Power":
                    if (player.power < 3)
                    {
                        player.score += 500;
                        player.power++;
                        Destroy(gameObject);
                    }

                    Debug.Log("파워충돌확인");
                    break;
                case "Boom":
                    if (player.boomSlot < 3)
                    {
                        player.score += 500;
                        player.boomSlot++;
                        Destroy(gameObject);
                    }

                    Debug.Log($"폭탄충돌확인 BoomSlot: {player.boomSlot}");
                    break;
            }
        }
    }
}