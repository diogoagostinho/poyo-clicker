using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GogetaBossManager : MonoBehaviour
{
    public static GogetaBossManager Instance;

    [Header("Boss")]
    public Sprite gogetaSprite;
    public AudioClip gogetaMusic;
    public float maxHealth = 5000f;

    float health;

    [Header("References")]
    public BossBarUI bossBar;
    public Image clickerImage;
    public AudioSource musicSource;
    public TextMeshProUGUI bossObjectiveText;

    Sprite previousSprite;
    AudioClip previousMusic;
    public bool IsActive { get; private set; }

    Vector2 originalClickerSize;
    public float gogetaSizeMultiplier = 3f;

    void Awake()
    {
        Instance = this;
    }

    public void StartGogetaFight()
    {
        IsActive = true;

        // Freeze systems
        GameLockManager.Instance.LockForSecretBoss();

        health = maxHealth;

        originalClickerSize = clickerImage.rectTransform.sizeDelta;

        previousSprite = clickerImage.sprite;
        previousMusic = musicSource.clip;

        clickerImage.sprite = gogetaSprite;
        clickerImage.rectTransform.sizeDelta = originalClickerSize * gogetaSizeMultiplier;

        musicSource.clip = gogetaMusic;
        musicSource.loop = true;
        musicSource.Play();

        bossObjectiveText.text = "Defeat Gogeta";
        bossObjectiveText.gameObject.SetActive(true);

        bossBar.SetProgress(1f);
    }

    public void Damage(float amount)
    {
        health -= amount;
        bossBar.SetProgress(health / maxHealth);

        if (health <= 0)
            EndFight();
    }

    void EndFight()
    {
        IsActive = false;

        clickerImage.sprite = previousSprite;
        clickerImage.rectTransform.sizeDelta = originalClickerSize;

        musicSource.clip = previousMusic;
        musicSource.Play();

        bossObjectiveText.gameObject.SetActive(false);
        bossBar.SetProgress(0f);

        DragonBallManager.Instance.ClearCollectedUI();

        GameLockManager.Instance.UnlockAfterSecretBoss();
    }
}
