using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BossManager : MonoBehaviour
{
    public ClickerManager clickerManager;
    Vector2 originalClickerSize2d;

    [Header("UI")]
    public BossBarUI bossBar;
    public Image clickerImage;   // your "Poyo"

    [Header("Boss UI")]
    public TextMeshProUGUI bossObjectiveText;

    [Header("Music")]
    public AudioSource musicSource;
    AudioClip previousMusic;
    public AudioClip originalMusic;

    [Header("Bosses")]
    public List<BossData> bosses;

    [Header("Rewards")]
    public int bossRewardBase = 1000;
    public float rewardMultiplier = 1.5f;

    float chargeProgress = 0f; // 0 -> 1
    float bossHealth = 0f;
    float bossMaxHealth = 100f;

    int bossLevel = 0;
    bool bossActive = false;

    string currentBossName;

    Sprite originalClickerSprite;

    public bool IsBossActive => bossActive;

    void Start()
    {
        originalClickerSprite = clickerImage.sprite;
        previousMusic = musicSource.clip;
        originalClickerSize2d = clickerImage.rectTransform.sizeDelta;
    }

    // CALLED BY CLICKER MANAGER
    public void AddPoints(float amount)
    {
        // Points are ALWAYS gained
        clickerManager.points += Mathf.RoundToInt(amount);
        clickerManager.UpdatePointsText();

        if (bossActive)
        {
            DamageBoss(amount);
        }
        else
        {
            ChargeBoss(amount);
        }
    }

    // ---------------- CHARGING ----------------

    void ChargeBoss(float points)
    {
        float difficulty = 1f + bossLevel * 0.25f;

        chargeProgress += points / (100f * difficulty);
        chargeProgress = Mathf.Clamp01(chargeProgress);

        bossBar.SetProgress(chargeProgress);

        if (chargeProgress >= 1f)
        {
            SummonBoss();
        }
    }

    // ---------------- BOSS ----------------

    void SummonBoss()
    {
        bossActive = true;
        chargeProgress = 1f;

        BossData boss = bosses[Random.Range(0, bosses.Count)];
        currentBossName = boss.bossName;

        if (bossObjectiveText != null)
        {
            bossObjectiveText.text = $"Defeat {currentBossName}";
            bossObjectiveText.gameObject.SetActive(true);
        }

        bossMaxHealth = boss.baseHealth * Mathf.Pow(boss.healthMultiplier, bossLevel);
        bossHealth = bossMaxHealth;

        clickerImage.sprite = boss.bossSprite;
        clickerImage.rectTransform.sizeDelta = originalClickerSize2d * 3f;

        previousMusic = musicSource.clip;
        musicSource.clip = boss.bossMusic;
        musicSource.loop = true;
        musicSource.Play();

        bossBar.SetProgress(1f);
    }

    void DamageBoss(float damage)
    {
        bossHealth -= damage;

        UpdateBossBar();

        if (bossHealth <= 0f)
        {
            DefeatBoss();
        }
    }

    void UpdateBossBar()
    {
        bossBar.SetProgress(bossHealth / bossMaxHealth);
    }

    void DefeatBoss()
    {
        bossActive = false;

        if (bossObjectiveText != null)
        {
            bossObjectiveText.gameObject.SetActive(false);
        }

        bossHealth = 0f;
        bossMaxHealth = 0f;

        chargeProgress = 0f;
        bossBar.SetProgress(0f);

        clickerImage.sprite = originalClickerSprite;
        clickerImage.rectTransform.sizeDelta = originalClickerSize2d;


        musicSource.clip = previousMusic != null ? previousMusic : originalMusic;
        musicSource.loop = true;
        musicSource.Play();

        int reward = Mathf.RoundToInt(
            bossRewardBase * Mathf.Pow(rewardMultiplier, bossLevel)
        );

        clickerManager.points += reward;
        clickerManager.UpdatePointsText();

        bossLevel++;
    }
}
