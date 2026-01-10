using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public ClickerManager clickerManager;
    Vector2 originalClickerSize2d;

    [Header("UI")]
    public BossBarUI bossBar;
    public Image clickerImage;   // your "Poyo"

    [Header("Boss Charge Difficulty")]
    public float baseChargeRequirement = 100f;
    public float chargeDifficultyMultiplier = 1.35f;

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
    public CustomizeManager customizeManager;

    Sprite originalClickerSprite;

    public bool IsBossActive => bossActive;
    public UIFlipController uiFlipController;

    [Header("Points UI")]
    public Image pointsIcon;
    Sprite originalPointsIcon;

    bool isSecondPhase = false;
    BossData currentBossData;

    bool isDioActive = false;
    Material originalBackgroundMaterial;

    [Header("Background")]
    public Image backgroundImage;

    void Start()
    {
        previousMusic = musicSource.clip;
        originalClickerSize2d = clickerImage.rectTransform.sizeDelta;
    }

    // CALLED BY CLICKER MANAGER
    public void AddPoints(float amount)
    {
        // STOP ALL POINTS & IDLE DURING DIO
        if (isDioActive)
            return;

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
        float required = baseChargeRequirement *
                         Mathf.Pow(chargeDifficultyMultiplier, bossLevel);

        chargeProgress += points / required;
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

        previousMusic = musicSource.clip;

        BossData boss = bosses[Random.Range(0, bosses.Count)];
        currentBossName = boss.bossName;

        Debug.Log("ID entity: " + boss.GetEntityId());

        currentBossData = boss;
        isSecondPhase = false;
        isDioActive = boss.isDio;

        DragonBallManager.Instance?.ClearActiveBall();

        if (boss.flipsUI)
        {
            uiFlipController.FlipUI();
        }

        if (bossObjectiveText != null)
        {
            bossObjectiveText.text = $"Derrota {currentBossName}";
            bossObjectiveText.gameObject.SetActive(true);
        }

        bossMaxHealth = boss.baseHealth * Mathf.Pow(boss.healthMultiplier, bossLevel);
        bossHealth = bossMaxHealth;

        clickerImage.sprite = boss.bossSprite;
        clickerImage.rectTransform.sizeDelta = originalClickerSize2d * 3f;

        if (boss.isDio && boss.dioGrayscaleMaterial != null)
        {
            originalBackgroundMaterial = backgroundImage.material;
            backgroundImage.material = boss.dioGrayscaleMaterial;
        }

        if (boss.isDio)
        {
            GameLockManager.Instance.LockForDio();
        }

        originalPointsIcon = pointsIcon.sprite;
        if (boss.drainPointsOnStart)
        {
            ApplyPointDrainBoss(boss);
        }
        
        if (boss.isDio && boss.spawnSfx != null)
        {
            StartCoroutine(PlayDioIntro(boss));
        }
        else
        {
            musicSource.clip = boss.bossMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        bossBar.SetProgress(1f);
    }

    IEnumerator PlayDioIntro(BossData boss)
    {
        musicSource.Stop();
        musicSource.loop = false;
        musicSource.clip = boss.spawnSfx;
        musicSource.Play();

        yield return new WaitForSeconds(boss.spawnSfx.length);

        musicSource.Stop();
        musicSource.clip = boss.bossMusic;
        musicSource.loop = true;
        musicSource.Play();
    }


    void ApplyPointDrainBoss(BossData boss)
    {
        // Change icon
        if (boss.pointsIconOverride != null)
            pointsIcon.sprite = boss.pointsIconOverride;

        // Drain points safely
        int drainAmount = Random.Range(
            boss.minPointDrain,
            boss.maxPointDrain + 1
        );

        clickerManager.points -= drainAmount;

        if (clickerManager.points < 0)
            clickerManager.points = 0;

        clickerManager.UpdatePointsText();
    }


    public void DamageBoss(float damage)
    {
        bossHealth -= damage;

        UpdateBossBar();

        if (bossHealth <= 0f)
        {
            if (currentBossData.hasSecondPhase && !isSecondPhase)
            {
                TriggerSecondPhase();
            }
            else
            {
                DefeatBoss();
            }
        }
    }

    void TriggerSecondPhase()
    {
        isSecondPhase = true;

        // Increase health
        bossMaxHealth *= currentBossData.secondPhaseHealthMultiplier;
        bossHealth = bossMaxHealth;

        // Change sprite
        if (currentBossData.secondPhaseSprite != null)
            clickerImage.sprite = currentBossData.secondPhaseSprite;

        // Change name
        if (bossObjectiveText != null)
        {
            bossObjectiveText.text = $"Derrota {currentBossData.secondPhaseName}";
        }

        bossBar.SetProgress(1f);
    }

    public bool IsDioActive()
    {
        return bossActive && isDioActive;
    }

    void UpdateBossBar()
    {
        bossBar.SetProgress(bossHealth / bossMaxHealth);
    }

    void DefeatBoss()
    {
        if (currentBossData != null && !string.IsNullOrEmpty(currentBossData.bossName))
        {
            TaskProgressManager.Instance.MarkBossDefeated(currentBossData.bossID);
        }

        bossActive = false;

        uiFlipController.ResetUI();

        isSecondPhase = false;
        currentBossData = null;

        if (isDioActive)
        {
            GameLockManager.Instance.UnlockAfterDio();
            backgroundImage.material = originalBackgroundMaterial;
            isDioActive = false;
        }

        if (bossObjectiveText != null)
        {
            bossObjectiveText.gameObject.SetActive(false);
        }

        RestorePointDrainBoss();

        bossHealth = 0f;
        bossMaxHealth = 0f;

        chargeProgress = 0f;
        bossBar.SetProgress(0f);

        customizeManager.ReapplyCurrentSkin();
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

    public void HideBossObjective()
    {
        bossObjectiveText.gameObject.SetActive(false);
    }

    void RestorePointDrainBoss()
    {
        if (pointsIcon != null && originalPointsIcon != null)
            pointsIcon.sprite = originalPointsIcon;
    }

    public void ResetBossProgress()
    {
        bossActive = false;
        bossLevel = 0;

        chargeProgress = 0f;
        bossHealth = 0f;
        bossMaxHealth = 100f;

        bossBar.SetProgress(0f);
        HideBossObjective();

        if (customizeManager != null)
        {
            customizeManager.ApplyCurrentSelection(CustomizeItemType.Skin);
            customizeManager.ApplyCurrentSelection(CustomizeItemType.Music);
        }

        clickerImage.rectTransform.sizeDelta = originalClickerSize2d;

        musicSource.loop = true;
    }

}
