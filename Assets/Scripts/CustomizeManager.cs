using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeManager : MonoBehaviour
{
    [Header("Data")]
    public List<CustomizeItemData> allItems;

    [Header("External References")]
    public ClickerManager clickerManager;
    public Image clickerImage;
    public UnityEngine.UI.Image backgroundImage;
    public AudioSource musicSource;

    // runtime state
    private HashSet<string> unlocked = new HashSet<string>();
    private Dictionary<CustomizeItemType, string> selected =
        new Dictionary<CustomizeItemType, string>();

    void Start()
    {
        InitializeDefaults();
    }

    void InitializeDefaults()
    {
        foreach (var item in allItems)
        {
            if (!item.isDefault) continue;

            unlocked.Add(item.itemId);
            selected[item.itemType] = item.itemId;
            ApplyItem(item);
        }
    }

    // --------- PUBLIC API ---------

    public bool IsUnlocked(CustomizeItemData item)
    {
        return unlocked.Contains(item.itemId);
    }

    public bool IsSelected(CustomizeItemData item)
    {
        return selected.TryGetValue(item.itemType, out var id)
               && id == item.itemId;
    }

    public bool TryBuy(CustomizeItemData item)
    {
        if (IsUnlocked(item)) return true;

        if (clickerManager.points < item.cost)
            return false;

        clickerManager.points -= item.cost;
        unlocked.Add(item.itemId);
        clickerManager.UpdatePointsText();
        return true;
    }

    public void Select(CustomizeItemData item)
    {
        selected[item.itemType] = item.itemId;
        ApplyItem(item);
    }

    // --------- APPLY ---------

    void ApplyItem(CustomizeItemData item)
    {
        switch (item.itemType)
        {
            case CustomizeItemType.Skin:
                if (item.skinSprite != null)
                    clickerImage.sprite = item.skinSprite;
                break;

            case CustomizeItemType.Background:
                if (item.backgroundSprite != null)
                    backgroundImage.sprite = item.backgroundSprite;
                break;

            case CustomizeItemType.Music:
                if (item.musicClip != null)
                {
                    musicSource.clip = item.musicClip;
                    musicSource.Play();
                }
                break;
        }
    }
}
