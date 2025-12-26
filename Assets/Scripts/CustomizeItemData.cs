using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomizeItem", menuName = "Customize/Item")]
public class CustomizeItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int cost;
    [TextArea(2, 5)]
    public string description;

    public CustomizeItemType itemType;

    public bool unlocked = false;
}
