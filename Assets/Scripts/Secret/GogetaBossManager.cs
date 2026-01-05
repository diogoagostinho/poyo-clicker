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

        previousSprite = clickerImage.sprite;
        previousMusic = musicSource.clip;

        clickerImage.sprite = gogetaSprite;

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
        musicSource.clip = previousMusic;
        musicSource.Play();

        bossObjectiveText.gameObject.SetActive(false);
        bossBar.SetProgress(0f);

        GameLockManager.Instance.UnlockAfterSecretBoss();
    }
}
