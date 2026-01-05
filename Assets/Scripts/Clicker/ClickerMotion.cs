using UnityEngine;

public class ClickerMotion : MonoBehaviour
{
    [Header("Scale (Breathing)")]
    public float scaleAmount = 0.05f;
    public float scaleSpeed = 1.5f;

    [Header("Rotation (Wiggle)")]
    public float rotationAmount = 5f;
    public float rotationSpeed = 1f;

    Vector3 startScale;
    Quaternion startRotation;

    float clickBoost = 0f;

    void Start()
    {
        startScale = transform.localScale;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Breathing scale
        float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        float boost = clickBoost;
        transform.localScale = startScale + Vector3.one * (scaleOffset + boost);

        // Gentle rotation
        float rotationOffset = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        transform.rotation = startRotation * Quaternion.Euler(0, 0, rotationOffset);

        // Fade click boost
        clickBoost = Mathf.Lerp(clickBoost, 0f, Time.deltaTime * 8f);
    }

    public void OnClick()
    {
        clickBoost = 0.1f;
    }
}
