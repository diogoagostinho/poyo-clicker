using UnityEngine;

public class DataWipeManager : MonoBehaviour
{
    public PrestigeManager prestigeManager;
    public CustomizeManager customizeManager;
    public ClickerManager clickerManager;
    public IdleUpgrade idleUpgrade;
    public BossManager bossManager;
    public TaskProgressManager taskProgressManager;
    public DragonBallManager dragonBallManager;

    public void WipeAllData()
    {
        // Clear ALL PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Reset prestige system
        prestigeManager?.ResetPrestigeData();

        // Reset customize unlocks + selection
        customizeManager?.ResetAllCustomizeData();

        // Reset clicker stats
        clickerManager?.ResetProgress();

        // Reset idle upgrade
        idleUpgrade?.ResetUpgrade();

        // Reset boss system
        bossManager?.ResetBossProgress();

        // Reset task progress
        taskProgressManager?.ResetAllProgress();

        // RESET DRAGON BALLS
        dragonBallManager?.ResetDragonBalls();

        // Reload scene for a fully fresh start
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
