using UnityEngine;

public class DataWipeManager : MonoBehaviour
{
    public PrestigeManager prestigeManager;
    public CustomizeManager customizeManager;
    public ClickerManager clickerManager;
    public IdleUpgrade idleUpgrade;
    public BossManager bossManager;
    public TaskProgressManager taskProgressManager;

    public void WipeAllData()
    {
        // Clear ALL PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Reset prestige system
        if (prestigeManager != null)
            prestigeManager.ResetPrestigeData();

        // Reset customize unlocks + selection
        if (customizeManager != null)
            customizeManager.ResetAllCustomizeData();

        // Reset clicker stats
        if (clickerManager != null)
            clickerManager.ResetProgress();

        // Reset idle upgrade
        if (idleUpgrade != null)
            idleUpgrade.ResetUpgrade();

        // Reset boss system
        if (bossManager != null)
            bossManager.ResetBossProgress();

        // Reset task progress (boss defeated list, etc.)
        if (taskProgressManager != null)
            taskProgressManager.ResetAllProgress();

        // Reload scene for a fully fresh start
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
