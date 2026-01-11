using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    string savePath;
    public GameSaveData data = new GameSaveData();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save.json";
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------- SAVE ----------------
    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    // ---------------- LOAD ----------------
    public void Load()
    {
        if (!File.Exists(savePath))
        {
            data = new GameSaveData(); // new save
            Save();
            return;
        }

        string json = File.ReadAllText(savePath);
        data = JsonUtility.FromJson<GameSaveData>(json);
    }

    // ---------------- RESET ----------------
    public void WipeData()
    {
        data = new GameSaveData();
        Save();
    }
}
