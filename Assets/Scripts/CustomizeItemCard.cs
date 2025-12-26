using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeItemCard : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI itemNameText;
    public Image iconImage;
    public TextMeshProUGUI costText;
    public Image currencyIcon;
    public TextMeshProUGUI descriptionText;
    public Button cardButton; // whole card is clickable

    [HideInInspector]
    public CustomizeItemData itemData;

    private ClickerManager clickerManager;
    private CustomizeManager customizeManager;

    [Header("Background")]
    public Image cardBackground; // assign the panel/image that is the card background
    private Color lockedColor = new Color32(0xDC, 0x14, 0x3C, 0xFF);     // #DC143C
    private Color unlockedColor = new Color32(0xE4, 0x48, 0x91, 0xFF);   // #E44891
    private Color selectedColor = new Color32(0x13, 0xDC, 0xB4, 0xFF);   // #13DCB4

    private Coroutine colorTransitionCoroutine;

    public void Initialize(CustomizeItemData data, ClickerManager clicker, CustomizeManager manager)
    {
        itemData = data;
        clickerManager = clicker;
        customizeManager = manager;

        /*
        itemNameText.text = data.itemName;
        iconImage.sprite = data.icon;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
        */

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnCardClicked);

        UpdateUI();
    }

    public void UpdateUI()
    {
        //string previousText = costText.text;

        itemNameText.text = itemData.itemName;
        iconImage.sprite = itemData.icon;
        descriptionText.text = itemData.description;

        //FOR TESTING
        cardButton.interactable = true;

        if (!customizeManager.IsUnlocked(itemData))
        {
            costText.text = itemData.cost.ToString();
            //cardButton.interactable = clickerManager.points >= itemData.cost;
        }
        else
        {
            bool isSelected = customizeManager.IsItemSelected(itemData);
            costText.text = customizeManager.IsItemSelected(itemData) ? "Selected" : "Unlocked";

            //cardButton.interactable = true;

            StartColorTransition(customizeManager.IsItemSelected(itemData) ? selectedColor : unlockedColor);
        }

        /*
        if (!itemData.unlocked)
        {
            costText.text = itemData.cost.ToString();
            cardButton.interactable = clickerManager.points >= itemData.cost;
        }
        else
        {
            bool isSelected = customizeManager.IsItemSelected(itemData);
            costText.text = isSelected ? "Selected" : "Unlocked";
            cardButton.interactable = true;

            StartColorTransition(isSelected ? selectedColor : unlockedColor);
        }

        /*
        if (!itemData.unlocked)
        {
            costText.text = itemData.cost.ToString();
            currencyIcon.gameObject.SetActive(true);
            cardButton.interactable = clickerManager.points >= itemData.cost;

            StartColorTransition(lockedColor);
        }
        else
        {
            bool isSelected = customizeManager.IsItemSelected(itemData);

            costText.text = isSelected ? "Selected" : "Unlocked";
            currencyIcon.gameObject.SetActive(true);
            cardButton.interactable = true;

            StartColorTransition(isSelected ? selectedColor : unlockedColor);
        }
        */
    }

    private void StartColorTransition(Color targetColor, float duration = 0.2f)
    {
        // Only start coroutine if the GameObject is active
        if (!gameObject.activeInHierarchy)
        {
            // Directly set the color without fading
            cardBackground.color = targetColor;
            return;
        }

        if (colorTransitionCoroutine != null)
            StopCoroutine(colorTransitionCoroutine);

        colorTransitionCoroutine = StartCoroutine(FadeColor(targetColor, duration));
    }

    private IEnumerator FadeColor(Color targetColor, float duration)
    {
        Color startColor = cardBackground.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cardBackground.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }

        cardBackground.color = targetColor;
    }

    private void OnCardClicked()
    {
        Debug.Log($"Clicked on {itemData.itemName}. Unlocked={itemData.unlocked}, Points={clickerManager.points}, Cost={itemData.cost}");

        // Use runtime unlocked state
        if (!customizeManager.IsUnlocked(itemData))// && clickerManager.points >= itemData.cost)
        {
            if (clickerManager.points >= itemData.cost)
            {
                clickerManager.points -= itemData.cost;
                customizeManager.UnlockItem(itemData);
                clickerManager.UpdatePointsText();
            }
            else
            {
                Debug.Log("Not enough points to unlock " + itemData.itemName);
                return; // early exit
            }
            /*
            clickerManager.points -= itemData.cost;
            customizeManager.UnlockItem(itemData);
            clickerManager.UpdatePointsText();*/
        }

        customizeManager.SelectItem(itemData);
        UpdateUI();

        /*
        if (!itemData.unlocked && clickerManager.points >= itemData.cost)
        {
            clickerManager.points -= itemData.cost;
            itemData.unlocked = true;
            clickerManager.UpdatePointsText();
        }

        customizeManager.SelectItem(itemData);
        UpdateUI();

        /*
        if (!customizeManager.IsUnlocked(itemData) && clickerManager.points >= itemData.cost)
        {
            clickerManager.points -= itemData.cost;
            customizeManager.UnlockItem(itemData);
            clickerManager.UpdatePointsText();
        }

        customizeManager.SelectItem(itemData);
        UpdateUI();
        */
    }
}
