using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeManager : MonoBehaviour
{
    public ClickerManager clickerManager;
    public Transform skinsContent;
    public Transform backgroundsContent;
    public Transform musicContent;
    public GameObject itemCardPrefab;
    public List<CustomizeItemData> allItems;

    private Dictionary<CustomizeItemType, CustomizeItemData> selectedItems = new Dictionary<CustomizeItemType, CustomizeItemData>();

    [Header("Tabs")]
    public GameObject skinsContentObject;
    public GameObject backgroundsContentObject;
    public GameObject musicContentObject;

    [Header("Tab Buttons")]
    public Button skinsTabButton;
    public Button backgroundsTabButton;
    public Button musicTabButton;
    public Color activeColor = new Color32(0xE4, 0x48, 0x91, 0xFF);   // #E44891
    public Color inactiveColor = Color.gray;

    private HashSet<CustomizeItemData> unlockedItems = new HashSet<CustomizeItemData>();
    private CustomizeItemData selectedItem;
    

    public bool IsUnlocked(CustomizeItemData item)
    {
        return unlockedItems.Contains(item);
    }

    public void UnlockItem(CustomizeItemData item)
    {
        unlockedItems.Add(item);
    }

    void Start()
    {
        if (clickerManager == null)
        {
            Debug.LogError("ClickerManager not assigned in CustomizeManager!");
            return;
        }

        if (skinsContent == null || backgroundsContent == null || musicContent == null)
        {
            Debug.LogError("One or more content parents are not assigned in CustomizeManager!");
            return;
        }

        if (itemCardPrefab == null)
        {
            Debug.LogError("ItemCardPrefab is not assigned!");
            return;
        }

        // Initialize tabs
        ShowTab(CustomizeItemType.Skin);

        // Hook up buttons
        skinsTabButton.onClick.AddListener(() => ShowTab(CustomizeItemType.Skin));
        backgroundsTabButton.onClick.AddListener(() => ShowTab(CustomizeItemType.Background));
        musicTabButton.onClick.AddListener(() => ShowTab(CustomizeItemType.Music));

        SpawnItems();
        InitializeDefaultSelections(allItems);
    }

    public void ShowTab(CustomizeItemType type)
    {
        // Hide all content
        skinsContentObject.SetActive(false);
        backgroundsContentObject.SetActive(false);
        musicContentObject.SetActive(false);

        // Reset button colors
        skinsTabButton.image.color = inactiveColor;
        backgroundsTabButton.image.color = inactiveColor;
        musicTabButton.image.color = inactiveColor;

        // Show selected content and highlight button
        switch (type)
        {
            case CustomizeItemType.Skin:
                skinsContentObject.SetActive(true);
                skinsTabButton.image.color = activeColor;
                break;
            case CustomizeItemType.Background:
                backgroundsContentObject.SetActive(true);
                backgroundsTabButton.image.color = activeColor;
                break;
            case CustomizeItemType.Music:
                musicContentObject.SetActive(true);
                musicTabButton.image.color = activeColor;
                break;
        }


    }

    public bool IsItemSelected(CustomizeItemData item)
    {
        /*
        if (!selectedItems.TryGetValue(item.itemType, out var selected))
            return false;*/

        return selectedItems.TryGetValue(item.itemType, out var selected) && selected == item;
    }

    void SpawnItems()
    {
        foreach (var item in allItems)
        {
            Transform parent = item.itemType switch
            {
                CustomizeItemType.Skin => skinsContent,
                CustomizeItemType.Background => backgroundsContent,
                CustomizeItemType.Music => musicContent,
                _ => null
            };

            if (parent == null) continue;

            // Make parent active temporarily if it was inactive
            bool wasInactive = !parent.gameObject.activeInHierarchy;
            if (wasInactive) parent.gameObject.SetActive(true);

            GameObject card = Instantiate(itemCardPrefab, parent);
            card.SetActive(true);

            card.GetComponent<CustomizeItemCard>().Initialize(item, clickerManager, this);

            /*
            var cardScript = card.GetComponent<CustomizeItemCard>();
            cardScript.Initialize(item, clickerManager, this);
            */

            // Restore parent state
            if (wasInactive) parent.gameObject.SetActive(false);

            /*

            GameObject card = Instantiate(itemCardPrefab, parent);
            card.SetActive(true); // ensures Button works
            var cardScript = card.GetComponent<CustomizeItemCard>();
            cardScript.Initialize(item, clickerManager, this);

            /*

            GameObject card = Instantiate(itemCardPrefab, parent);

            // FIX: ensure card is active so Button will work
            card.SetActive(true);

            var cardScript = card.GetComponent<CustomizeItemCard>();

            if (cardScript != null)
                cardScript.Initialize(item, clickerManager, this);
            else
                Debug.LogError("ItemCardPrefab missing CustomizeItemCard script!");
            */
        }
    }

    public void SelectItem(CustomizeItemData item)
    {
        selectedItems[item.itemType] = item;

        /*

        // Apply visuals
        switch (item.itemType)
        {
            case CustomizeItemType.Skin:
                // Replace your main clicker sprite
                break;
            case CustomizeItemType.Background:
                // Replace background image
                break;
            case CustomizeItemType.Music:
                // Replace background music
                break;
        }
        */
    }

    // Optional: get the currently selected item for a type
    public CustomizeItemData GetSelectedItem(CustomizeItemType type)
    {
        selectedItems.TryGetValue(type, out var selected);
        return selected;
    }

    public void InitializeDefaultSelections(List<CustomizeItemData> allItems)
    {
        foreach (var item in allItems)
        {
            if (!selectedItems.ContainsKey(item.itemType))
            {
                // Pick the first unlocked item of each type, or the first in the list
                SelectItem(item);
            }
        }
    }
}
