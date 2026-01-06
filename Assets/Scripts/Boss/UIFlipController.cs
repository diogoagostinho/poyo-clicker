using UnityEngine;

public class UIFlipController : MonoBehaviour
{
    RectTransform rectTransform;
    Quaternion normalRotation;
    Quaternion flippedRotation;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        normalRotation = Quaternion.identity;
        flippedRotation = Quaternion.Euler(0f, 0f, 180f);
    }

    public void FlipUI()
    {
        rectTransform.rotation = flippedRotation;
    }

    public void ResetUI()
    {
        rectTransform.rotation = normalRotation;
    }
}
