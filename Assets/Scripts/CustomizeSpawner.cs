using UnityEngine;

public class CustomizeSpawner : MonoBehaviour
{
    public CustomizeManager manager;
    public CustomizeItemCard cardPrefab;
    public Transform skinsContent;
    public Transform backgroundsContent;
    public Transform musicContent;

    void Start()
    {
        //DEBUG
        Debug.Log("CustomizeSpawner started");

        foreach (var item in manager.allItems)
        {
            Transform parent = GetParent(item.itemType);
            var card = Instantiate(cardPrefab, parent);
            card.Initialize(item, manager);
        }
    }

    Transform GetParent(CustomizeItemType type)
    {
        switch (type)
        {
            case CustomizeItemType.Skin: return skinsContent;
            case CustomizeItemType.Background: return backgroundsContent;
            case CustomizeItemType.Music: return musicContent;
        }
        return null;
    }
}
