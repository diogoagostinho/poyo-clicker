using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleUpgrade : MonoBehaviour
{
    public int level = 0;
    public int cost = 6;

    public float costMultiplier = 1.3f;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;

    public ClickerManager clickerManager;

    void Start()
    {
        RefreshUI();
    }

    public void OnClickUpgrade()
    {
        if (clickerManager.points < cost)
            return;

        clickerManager.points -= cost;

        level++;
        cost = Mathf.RoundToInt(cost * costMultiplier);

        RefreshUI();
    }

    void RefreshUI()
    {
        levelText.text = $"{level} / sec";
        costText.text = $"Cost: {cost}";
        clickerManager.UpdatePointsText();
    }

    public int GetPointsPerSecond()
    {
        return level;
    }
}
