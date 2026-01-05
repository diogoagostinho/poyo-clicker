using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigePanel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI prestigeCountText;
    public TextMeshProUGUI prestigeCostText;

    public Button closeButton;
    public Button activateButton;

    [Header("References")]
    public ClickerManager clickerManager;
    public PrestigeManager prestigeManager;

    void Awake()
    {
        closeButton.onClick.AddListener(Close);
        activateButton.onClick.AddListener(OnClickActivate);
    }

    void OnEnable()
    {
        RefreshUI();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        RefreshUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void RefreshUI()
    {
        messageText.text =
            "Queres fazer prestige e ativar o Killer Bean?\n" +
            "Isto vai resetar os teus pontos e upgrades mas vai aumentar o poder dos clicks\n\n" +
            "Os itens no customizar e as conquistas não serão afetados";

        prestigeCountText.text =
            $"Killer Bean foi ativado {prestigeManager.prestigeCount} vezes";

        prestigeCostText.text =
            $"Custo para o próximo prestige: {prestigeManager.currentPrestigeCost}";

        activateButton.interactable =
            clickerManager.points >= prestigeManager.currentPrestigeCost;
    }

    void OnClickActivate()
    {
        if (!prestigeManager.CanPrestige(clickerManager.points))
            return;

        prestigeManager.ExecutePrestige();
        Close();
    }
}
