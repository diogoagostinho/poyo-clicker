using UnityEngine;

public class IdleUpgrade : MonoBehaviour
{
    public int level = 0;
    public int cost = 100;
    public float costMultiplier = 1.3f;

    public IdleUpgradeTooltip tooltip;
    public ClickerManager clickerManager;
    public LevelUpTooltip levelUpTooltip;
    public PrestigeManager prestigeManager;

    public event System.Action OnUpgradeChanged;

    public void Upgrade()
    {
        if (clickerManager.points < cost)
            return;

        clickerManager.points -= cost;

        level++;
        cost = Mathf.RoundToInt(cost * costMultiplier);

        clickerManager.UpdatePointsText();

        if (tooltip != null)
            tooltip.RefreshTooltip();

        OnUpgradeChanged?.Invoke();
    }

    public int GetPointsPerSecond()
    {
        return level * (1 + prestigeManager.prestigeCount);
    }

    public void RefreshAfterPrestige()
    {
        clickerManager.UpdatePointsText();
        levelUpTooltip.RefreshTooltip();
    }

    public int GetEffectivePointsPerSecond()
    {
        return level + PrestigeManager.Instance.prestigeCount;
    }

    public void ResetUpgrade()
    {
        level = 0;
        cost = 100;

        if (tooltip != null)
            tooltip.RefreshTooltip();
    }

}