using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] lifeImages;
    public Image[] boomImages;
    private GameObject player;
    public TextMeshProUGUI scoreText;
    public GameObject background;
    public GameObject GameOverSet;

    void OnEnable()
    {
        GameManager.OnGameOver += ShowGameOver;
        GameManager.OnRestart += HideGameOver;
    }

    void OnDisable()
    {
        GameManager.OnGameOver -= ShowGameOver;
        GameManager.OnRestart -= HideGameOver;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        GameOverSet.SetActive(false);
    }

    void Update()
    {
        if (player == null || GameManager.Instance.State != GameState.Playing) return;

        UpdateLifeIcon();
        UpdateBoomIcon();

        Player playerScript = player.GetComponent<Player>();
        scoreText.text = playerScript.score.ToString("N0");
    }

    public void UpdateLifeIcon()
    {
        Player playerScript = player.GetComponent<Player>();
        int life = playerScript.life;
        for (int i = 0; i < 3; i++)
            lifeImages[i].color = new Color(1, 1, 1, 0);
        for (int i = 0; i < life; i++)
            lifeImages[i].color = new Color(1, 1, 1, 1);
    }

    public void UpdateBoomIcon()
    {
        Player playerScript = player.GetComponent<Player>();
        int boomSlot = playerScript.boomSlot;
        for (int i = 0; i < 3; i++)
            boomImages[i].color = new Color(1, 1, 1, 0);
        for (int i = 0; i < boomSlot; i++)
            boomImages[i].color = new Color(1, 1, 1, 1);
    }

    public void lifeMinus()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript.life > 0)
            playerScript.life--;
    }

    void ShowGameOver()
    {
        player.GetComponent<Player>().PlayerReset();
        background.SetActive(false);
        player.SetActive(false);
        SetHudActive(false);
        GameOverSet.SetActive(true);
    }

    void HideGameOver()
    {
        player.GetComponent<Player>().life = 3;
        background.SetActive(true);
        player.SetActive(true);
        SetHudActive(true);
        GameOverSet.SetActive(false);
    }

    void SetHudActive(bool active)
    {
        foreach (var img in lifeImages)
            img.gameObject.SetActive(active);
        foreach (var img in boomImages)
            img.gameObject.SetActive(active);
        scoreText.gameObject.SetActive(active);
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }
}
