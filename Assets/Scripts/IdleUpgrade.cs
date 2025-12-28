using UnityEngine;

public class IdleUpgrade : MonoBehaviour
{
    public int level = 0;
    public int cost = 100;
    public float costMultiplier = 1.3f;

    public IdleUpgradeTooltip tooltip;
    public ClickerManager clickerManager;

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
    }

    public int GetPointsPerSecond()
    {
        return level;
    }
}