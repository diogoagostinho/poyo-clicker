using UnityEngine;
using UnityEngine.UI;

public class BossBarUI : MonoBehaviour
{
    public Image fillImage;

    public void SetProgress(float normalized)
    {
        fillImage.fillAmount = Mathf.Clamp01(normalized);
    }
}
