using UnityEngine;
using UnityEngine.EventSystems;

public class DragonBall : MonoBehaviour, IPointerClickHandler
{
    public DragonBallData data;

    public void Init(DragonBallData newData)
    {
        data = newData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null)
        {
            Debug.LogError("DragonBall clicked without data!");
            return;
        }

        DragonBallManager.Instance.CollectBall(data);
        Destroy(gameObject);
    }
}
