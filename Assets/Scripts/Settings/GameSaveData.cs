using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    // -------------- SETTINGS --------------
    public int resolutionIndex;
    public bool fullscreen;
    public float audioVolume;

    // -------------- PLAYER PROGRESSION --------------
    public int prestigeCount;
    public int points;
    public int upgradeLevel;
    public int levelUpLevel;

    // -------------- CUSTOMIZATION --------------
    public List<string> unlockedItems = new List<string>();
    public Dictionary<string, string> selectedItems = new Dictionary<string, string>();

    // -------------- DRAGON BALLS --------------
    public List<DragonBallType> collectedDragonBalls = new();

    // -------------- BOSS PROGRESS --------------
    public Dictionary<string, int> bossDefeatCounts = new Dictionary<string, int>();
}
