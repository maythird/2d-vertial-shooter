using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image[] lifeImages;
    public Image[] boomImages;
    private Player player;
    public TextMeshProUGUI scoreText;
    public GameObject background;
    public GameObject GameOverSet;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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
        player = Player.Instance;
        GameOverSet.SetActive(false);
    }

    void Update()
    {
        if (player == null || GameManager.Instance.State != GameState.Playing) return;

        UpdateLifeIcon();
        UpdateBoomIcon();
        scoreText.text = player.score.ToString("N0");
    }

    public void UpdateLifeIcon()
    {
        for (int i = 0; i < 3; i++)
            lifeImages[i].color = new Color(1, 1, 1, 0);
        for (int i = 0; i < player.life; i++)
            lifeImages[i].color = new Color(1, 1, 1, 1);
    }

    public void UpdateBoomIcon()
    {
        for (int i = 0; i < 3; i++)
            boomImages[i].color = new Color(1, 1, 1, 0);
        for (int i = 0; i < player.boomSlot; i++)
            boomImages[i].color = new Color(1, 1, 1, 1);
    }

    public void lifeMinus()
    {
        if (player.life > 0)
            player.life--;
    }

    void ShowGameOver()
    {
        player.PlayerReset();
        background.SetActive(false);
        player.gameObject.SetActive(false);
        SetHudActive(false);
        GameOverSet.SetActive(true);
    }

    void HideGameOver()
    {
        player.life = 3;
        background.SetActive(true);
        player.gameObject.SetActive(true);
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
