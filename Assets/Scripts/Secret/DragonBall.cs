using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragonBall : MonoBehaviour, IPointerClickHandler
{
    public DragonBallData data;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Init(DragonBallData newData)
    {
        data = newData;
        image.sprite = data.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DragonBallManager.Instance.CollectBall(data);
        Destroy(gameObject);
    }
}
