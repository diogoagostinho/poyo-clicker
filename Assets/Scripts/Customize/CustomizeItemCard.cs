using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomizeItemCard : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    public TextMeshProUGUI nameText;
    public Image iconImage;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    public Image background;

    [Header("Colors")]
    public Color lockedColor = new Color32(220, 20, 60, 255);   // crimson
    public Color unlockedColor = new Color32(228, 72, 145, 255);
    public Color selectedColor = new Color32(19, 220, 180, 255);

    CustomizeItemData data;
    CustomizeManager manager;

    public void Initialize(CustomizeItemData item, CustomizeManager mgr)
    {
        data = item;
        manager = mgr;

        nameText.text = item.itemName;
        iconImage.sprite = item.icon;

        // DESCRIPTION LOGIC
        if (item.itemType == CustomizeItemType.Music)
        {
            descriptionText.gameObject.SetActive(false);
        }
        else
        {
            descriptionText.gameObject.SetActive(true);
            descriptionText.text = item.description;
        }

        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Clicked:
        if (data.unlockType != UnlockType.NormalPurchase &&
            !manager.MeetsTaskRequirement(data))
        {
            return; // still locked by task
        }

        if (!manager.TryBuy(data) && data.unlockType == UnlockType.NormalPurchase)
            return;

        manager.Select(data);
    }

    public void UpdateUI()
    {
        bool unlocked = manager.IsUnlocked(data);
        bool selected = manager.IsSelected(data);

        if (!unlocked)
        {
            bool unlockedNormally = manager.IsUnlocked(data);
            bool unlockedByTask = manager.MeetsTaskRequirement(data);

            if (!unlockedNormally && !unlockedByTask)
            {
                background.color = lockedColor;

                if (data.unlockType == UnlockType.NormalPurchase)
                    costText.text = data.cost.ToString();
                else
                    costText.text = manager.GetRequirementText(data);

                return;
            }
        }
        else if (selected)
        {
            background.color = selectedColor;
            costText.text = "Selecionado";
        }
        else
        {
            background.color = unlockedColor;

            if (data.unlockType == UnlockType.NormalPurchase)
                costText.text = "Desbloqueado";
            else
                costText.text = "Desbloqueado";
        }
    }
}
