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

        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!manager.TryBuy(data))
            return;

        manager.Select(data);
    }

    public void UpdateUI()
    {
        if (!manager.IsUnlocked(data))
        {
            background.color = lockedColor;
            costText.text = data.cost.ToString();
        }
        else if (manager.IsSelected(data))
        {
            background.color = selectedColor;
            costText.text = "Selected";
        }
        else
        {
            background.color = unlockedColor;
            costText.text = "Unlocked";
        }
    }
}
