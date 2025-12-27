using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickerManager : MonoBehaviour
{
    public int points = 0;
    public TextMeshProUGUI pointsText;
    public AudioSource clickSound;

    [Header("Idle Upgrades")]
    public IdleUpgrade idleUpgrade;

    [Header("Click Power")]
    public int clickPower = 1;

    [Header("Level Up")]
    public int levelUpCost = 10;
    public TextMeshProUGUI levelUpText;

    public LevelUpTooltip levelUpTooltip;

    [Header("Boss System")]
    public BossManager bossManager;

    void Start()
    {
        UpdatePointsText();
        StartCoroutine(IdleTick());
    }

    IEnumerator IdleTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            int idleIncome = idleUpgrade.GetPointsPerSecond();
            if (idleIncome > 0)
            {
                GainPoints(idleIncome);
            }
        }
    }

    public void AddPoints()
    {
        GainPoints(clickPower);

        clickSound.pitch = Random.Range(0.75f, 1.25f);
        clickSound.Play();
    }

    public void UpdatePointsText()
    {
        pointsText.text = "" + points;
    }

    public void GainPoints(int amount)
    {
        // BossManager decides what happens with the gain
        bossManager.AddPoints(amount);
    }


    void Update()
    {
        // DEBUG: Press J to add 100 money
        if(Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            GainPoints(100);
        }
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            GainPoints(1000);
        }
    }

    public void LevelUpClick()
    {
        if (points < levelUpCost)
            return;

        points -= levelUpCost;
        clickPower++;
        levelUpCost = Mathf.RoundToInt(levelUpCost * 1.5f);

        UpdatePointsText();

        // Refresh tooltip if it's visible
        if (levelUpTooltip != null)
            levelUpTooltip.RefreshTooltip();
    }
}
