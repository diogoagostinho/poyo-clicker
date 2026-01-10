using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class TaskProgressManager : MonoBehaviour
{
    public static TaskProgressManager Instance;

    // --- Data to Save ---
    public int prestigeCount = 0;

    // BossID -> times defeated
    private Dictionary<string, int> bossDefeatCount = new Dictionary<string, int>();
    private Dictionary<string, int> specificBossKills = new Dictionary<string, int>();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------- SAVE / LOAD ----------------
    [System.Serializable]
    private class SaveData
    {
        public int prestigeCount;
        public List<string> bossIds = new();
        public List<int> bossCounts = new();
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.prestigeCount = prestigeCount;

        foreach (var kv in bossDefeatCount)
        {
            data.bossIds.Add(kv.Key);
            data.bossCounts.Add(kv.Value);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("TaskProgress", json);
        PlayerPrefs.Save();
    }

    void Load()
    {
        if (!PlayerPrefs.HasKey("TaskProgress"))
            return;

        string json = PlayerPrefs.GetString("TaskProgress");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        prestigeCount = data.prestigeCount;

        bossDefeatCount = new Dictionary<string, int>();
        for (int i = 0; i < data.bossIds.Count; i++)
            bossDefeatCount[data.bossIds[i]] = data.bossCounts[i];
    }

    // ---------------- API ----------------

    public void AddPrestige()
    {
        prestigeCount++;
        Save();
    }

    public void MarkBossDefeated(string bossId)
    {
        if (!bossDefeatCount.ContainsKey(bossId))
            bossDefeatCount[bossId] = 0;

        bossDefeatCount[bossId]++;
        Save();
    }

    public int GetBossDefeatCount(string bossId)
    {
        return bossDefeatCount.ContainsKey(bossId) ? bossDefeatCount[bossId] : 0;
    }

    public void ResetAllProgress()
    {
        bossDefeatCount.Clear();
        prestigeCount = 0;

        Save();
    }

    public void AddSpecificBossKill(string bossID)
    {
        if (!specificBossKills.ContainsKey(bossID))
            specificBossKills[bossID] = 0;

        specificBossKills[bossID]++;
        Save();
    }

    public int GetSpecificBossKillCount(string bossID)
    {
        if (specificBossKills.TryGetValue(bossID, out int count))
            return count;

        return 0;
    }


}
