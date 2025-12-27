using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BossManager : MonoBehaviour
{
    public ClickerManager clickerManager;

    [Header("UI")]
    public BossBarUI bossBar;
    public Image clickerImage;   // your "Poyo"

    [Header("Music")]
    public AudioSource musicSource;
    AudioClip previousMusic;

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

    Sprite originalClickerSprite;
    AudioClip originalMusic;

    void Start()
    {
        originalClickerSprite = clickerImage.sprite;
        originalMusic = musicSource.clip;
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
        bossBar.SetProgress(chargeProgress);

        // ADD POINTS HERE
        clickerManager.points += Mathf.RoundToInt(points);
        clickerManager.UpdatePointsText();

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

        bossMaxHealth = boss.baseHealth * Mathf.Pow(boss.healthMultiplier, bossLevel);
        bossHealth = bossMaxHealth;

        clickerImage.sprite = boss.bossSprite;

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
        bossHealth = 0f;
        bossMaxHealth = 0f;
        bossLevel++;

        chargeProgress = 0f;
        bossBar.SetProgress(0f);

        int reward = Mathf.RoundToInt(
            bossRewardBase * Mathf.Pow(rewardMultiplier, bossLevel)
        );

        // give reward
        clickerManager.GainPoints(reward);

        clickerImage.sprite = originalClickerSprite;

        if (previousMusic != null)
        {
            musicSource.clip = previousMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
