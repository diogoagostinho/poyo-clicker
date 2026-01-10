using UnityEngine;

public enum UnlockType
{
    NormalPurchase,
    PrestigeRequirement,
    BossRequirement
}

[CreateAssetMenu(menuName = "Customize/Item")]
public class CustomizeItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemId;
    public CustomizeItemType itemType;
    public bool isDefault;
    public UnlockType unlockType = UnlockType.NormalPurchase;

    [Header("UI")]
    public string itemName;
    [TextArea(2, 4)]
    public string description;
    public Sprite icon;
    public int cost;

    [Header("Apply Data")]
    public Sprite skinSprite;
    public Sprite backgroundSprite;
    public AudioClip musicClip;

    [Header("Custom Task")]
    public int requiredPrestigeCount;     
    public string requiredBossID;         
    public string customUnlockText;
    public AudioClip customClickSound;

}
