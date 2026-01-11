using UnityEngine;

public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager Instance { get; private set; }

    public int PrestigeCount { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [Header("Prestige Settings")]
    public int prestigeCount = 0;
    public int basePrestigeCost = 250000;
    public int currentPrestigeCost;

    [Header("References")]
    public ClickerManager clickerManager;
    public BossManager bossManager;
    public IdleUpgrade idleUpgrade;

    void Start()
    {
        currentPrestigeCost = basePrestigeCost;
    }

    public bool CanPrestige(int currentPoints)
    {
        return currentPoints >= currentPrestigeCost;
    }

    public void ExecutePrestige()
    {
        // Spend points
        clickerManager.points = 0;
        clickerManager.UpdatePointsText();

        // Increase prestige
        prestigeCount++;
        currentPrestigeCost *= 2;

        TaskProgressManager.Instance.AddPrestige();

        // Reset click power + level up cost
        clickerManager.clickPower = 1;
        clickerManager.levelUpCost = 10;

        // Reset idle upgrade
        idleUpgrade.level = 0;
        idleUpgrade.cost = 6;
        idleUpgrade.RefreshAfterPrestige();

        // Reset bosses
        bossManager.ResetBossProgress();

        DragonBallManager.Instance.ResetForPrestige();

        PrestigeVideoPlayer.Instance.PlayPrestigeVideo();

        SaveManager.Instance.data.prestigeCount = prestigeCount;
        SaveManager.Instance.Save();
    }

    public void ResetPrestigeData()
    {
        prestigeCount = 0;

        PlayerPrefs.SetInt("prestigeCount", 0);
        PlayerPrefs.Save();
    }

}
