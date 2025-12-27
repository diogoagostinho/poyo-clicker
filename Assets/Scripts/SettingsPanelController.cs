using System.Collections;
using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.15f;

    bool isOpen;
    Coroutine fadeCoroutine;

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        SetOpen(true);
    }

    public void Close()
    {
        SetOpen(false);
    }

    void SetOpen(bool open)
    {
        isOpen = open;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadePanel(open));
    }

    IEnumerator FadePanel(bool open)
    {
        float startAlpha = canvasGroup.alpha;
        float endAlpha = open ? 1f : 0f;

        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
