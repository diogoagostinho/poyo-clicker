using UnityEngine;

public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager Instance;

    public int prestigeCount = 0;
    public int basePrestigeCost = 250000;
    public int currentPrestigeCost;

    void Awake()
    {
        Instance = this;
        currentPrestigeCost = basePrestigeCost;
    }

    public bool CanPrestige(int points)
    {
        return points >= currentPrestigeCost;
    }
}
