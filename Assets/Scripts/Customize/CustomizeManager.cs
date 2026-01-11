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

    private Dictionary<CustomizeItemType, List<CustomizeItemCard>> cardsByType
        = new Dictionary<CustomizeItemType, List<CustomizeItemCard>>();

    public BossManager bossManager;

    void Start()
    {
        InitializeDefaults();

        var save = SaveManager.Instance.data;

        foreach (string id in save.unlockedItems)
            unlocked.Add(id);

        foreach (var pair in save.selectedItems)
        {
            CustomizeItemType type = (CustomizeItemType)System.Enum.Parse(
                typeof(CustomizeItemType), pair.Key);

            selected[type] = pair.Value;

            var item = allItems.Find(i => i.itemId == pair.Value);
            if (item != null)
                ApplyItem(item);
        }
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

    public void RegisterCard(CustomizeItemCard card, CustomizeItemType type)
    {
        if (!cardsByType.ContainsKey(type))
            cardsByType[type] = new List<CustomizeItemCard>();

        cardsByType[type].Add(card);
    }

    public void RefreshType(CustomizeItemType type)
    {
        if (!cardsByType.ContainsKey(type)) return;

        foreach (var card in cardsByType[type])
            card.UpdateUI();
    }

    public bool IsUnlocked(CustomizeItemData item)
    {
        // Default items always unlocked
        if (item.isDefault)
            return true;

        // Already unlocked normally
        if (unlocked.Contains(item.itemId))
            return true;

        // Task-unlocked?
        if (item.unlockType != UnlockType.NormalPurchase)
        {
            if (MeetsTaskRequirement(item))
            {
                unlocked.Add(item.itemId);
                return true;
            }
        }

        return false;
    }

    public bool IsSelected(CustomizeItemData item)
    {
        return selected.TryGetValue(item.itemType, out var id)
               && id == item.itemId;
    }

    public bool TryBuy(CustomizeItemData item)
    {
        if (bossManager != null && bossManager.IsBossActive)
            return false;

        // Task items cannot be bought
        if (item.unlockType != UnlockType.NormalPurchase)
            return false;

        // Already unlocked
        if (IsUnlocked(item))
            return true;

        // Not enough points
        if (clickerManager.points < item.cost)
            return false;

        clickerManager.points -= item.cost;
        unlocked.Add(item.itemId);
        clickerManager.UpdatePointsText();

        SaveManager.Instance.data.unlockedItems.Add(item.itemId);
        SaveManager.Instance.Save();

        return true;
    }

    public void Select(CustomizeItemData item)
    {
        if (bossManager != null && bossManager.IsBossActive)
            return;

        selected[item.itemType] = item.itemId;
        ApplyItem(item);
        RefreshType(item.itemType);

        SaveManager.Instance.data.selectedItems[item.itemType.ToString()] = item.itemId;
        SaveManager.Instance.Save();
    }

    // --------- APPLY ---------

    void ApplyItem(CustomizeItemData item)
    {
        switch (item.itemType)
        {
            case CustomizeItemType.Skin:
                // Change the clicker sprite
                if (item.skinSprite != null)
                    clickerImage.sprite = item.skinSprite;

                // Apply click sound override (NEW)
                if (item.customClickSound != null)
                    ClickSoundManager.Instance.SetClickSound(item.customClickSound);
                else
                    ClickSoundManager.Instance.ResetToDefault();
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

    public void ReapplyCurrentSkin()
    {
        if (!selected.TryGetValue(CustomizeItemType.Skin, out var skinId))
            return;

        CustomizeItemData skin = allItems.Find(i => i.itemId == skinId);
        if (skin != null)
        {
            ApplyItem(skin);
        }
           
    }

    public void ReapplyCurrentBackground()
    {
        if (!selected.TryGetValue(CustomizeItemType.Background, out var backgroundId))
            return;

        CustomizeItemData background = allItems.Find(i => i.itemId == backgroundId);
        if (background != null)
            ApplyItem(background);
    }

    public void ReapplyCurrentMusic()
    {
        if (!selected.TryGetValue(CustomizeItemType.Music, out var musicId))
            return;

        CustomizeItemData music = allItems.Find(i => i.itemId == musicId);
        if (music != null)
            ApplyItem(music);
    }

    public void ApplyCurrentSelection(CustomizeItemType type)
    {
        switch (type)
        {
            case CustomizeItemType.Skin:
                ReapplyCurrentSkin();
                break;

            case CustomizeItemType.Background:
                ReapplyCurrentBackground();
                break;

            case CustomizeItemType.Music:
                ReapplyCurrentMusic();
                break;
        }
    }

    // ---------------------- TASK CHECKS ----------------------

    public bool MeetsTaskRequirement(CustomizeItemData item)
    {
        switch (item.unlockType)
        {
            case UnlockType.PrestigeRequirement:
                return TaskProgressManager.Instance.prestigeCount >= item.requiredPrestigeCount;

            case UnlockType.BossRequirement:
                return TaskProgressManager.Instance.GetBossDefeatCount(item.requiredBossID) > 0;

            case UnlockType.LinkedUnlock:
                return unlocked.Contains(item.requiredItemID);

            default:
                return false;
        }
    }

    public string GetRequirementText(CustomizeItemData item)
    {
        if (!string.IsNullOrEmpty(item.customUnlockText))
            return item.customUnlockText;

        switch (item.unlockType)
        {
            case UnlockType.PrestigeRequirement:
                return $"Ativa o Killer Bean {item.requiredPrestigeCount}x";

            case UnlockType.BossRequirement:
                return $"Derrota {item.requiredBossID}";

            default:
                return item.cost.ToString();
        }
    }

    public void ResetAllCustomizeData()
    {
        unlocked.Clear();
        selected.Clear();

        InitializeDefaults();    // Reapply default skins/background/music
        RefreshType(CustomizeItemType.Skin);
        RefreshType(CustomizeItemType.Background);
        RefreshType(CustomizeItemType.Music);
    }


}
