using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image[] lifeImages;
    public Image[] boomImages;
    private GameObject player;
    public TextMeshProUGUI scoreText;
    private string score;

    public GameObject GameOverSet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            UiManager();
            UpdateLifeIcon();
            UpdateBoomIcon();

            Player playerScript = player.GetComponent<Player>();
            score = playerScript.score.ToString("N0");
            if (playerScript.life > 0)
            {
                scoreText.text = score;
            }
            else
            {
                scoreText.text = "";
            }
        }
    }

    public void UpdateLifeIcon()
    {
        Player playerScript = player.GetComponent<Player>();
        int life = playerScript.life;
        Debug.Log($"Player life :{life}");
        for (int i = 0; i < 3; i++)
        {
            lifeImages[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < life; i++)
        {
            lifeImages[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon()
    {
        Player playerScript = player.GetComponent<Player>();
        int boomSlot = playerScript.boomSlot;
        Debug.Log($"Player boomSlot :{boomSlot}");
        for (int i = 0; i < 3; i++)
        {
            boomImages[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < boomSlot; i++)
        {
            boomImages[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void lifeMinus()
    {
        Player playerScript = player.GetComponent<Player>();

        if (playerScript.life > 0)
        {
            playerScript.life--;
        }

        Debug.Log($"life :{playerScript.life}");
    }

    public void UiManager()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript.life <= 0)
        {
            playerScript.PlayerReset();
            GameObject[] Bullets = GameObject.FindGameObjectsWithTag("Bullets");
            GameObject[] EnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullets");
            GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < Bullets.Length; i++)
            {
                Destroy(Bullets[i]);
            }

            for (int i = 0; i < EnemyBullets.Length; i++)
            {
                Destroy(EnemyBullets[i]);
            }

            for (int i = 0; i < Enemy.Length; i++)
            {
                Destroy(Enemy[i]);
            }

            player.SetActive(false);
            GameOverSet.SetActive(true);
        }
        else
        {
            GameOverSet.SetActive(false);
            player.SetActive(true);
        }
    }

    public void Restart()
    {
        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript.life <= 0)
            {
                playerScript.life = 3;
            }
        }
    }
}