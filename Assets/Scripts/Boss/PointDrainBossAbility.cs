using UnityEngine;
using UnityEngine.UI;

public class PointDrainBossAbility : MonoBehaviour
{
    [Header("Icon Swap")]
    public Image pointsIcon;
    public Sprite bossPointsSprite;

    [Header("Point Drain")]
    public int minDrain = 100;
    public int maxDrain = 10000;

    Sprite originalSprite;
    ClickerManager clickerManager;

    void Awake()
    {
        clickerManager = FindFirstObjectByType<ClickerManager>();
    }

    // Called when the boss fight starts
    public void OnBossStart()
    {
        if (pointsIcon != null)
        {
            originalSprite = pointsIcon.sprite;
            pointsIcon.sprite = bossPointsSprite;
        }

        DrainPoints();
    }

    // Called when the boss fight ends
    public void OnBossEnd()
    {
        if (pointsIcon != null && originalSprite != null)
        {
            pointsIcon.sprite = originalSprite;
        }
    }

    void DrainPoints()
    {
        if (clickerManager == null)
            return;

        int drainAmount = Random.Range(minDrain, maxDrain + 1);

        clickerManager.points -= drainAmount;

        if (clickerManager.points < 0)
            clickerManager.points = 0;

        clickerManager.UpdatePointsText();
    }
}
