using System.Collections;
using UnityEngine;

public class CustomizePanelController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.1f;

    bool isOpen;
    Coroutine fadeCoroutine;

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadePanel(isOpen));
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
            float t = elapsed / fadeDuration;

            // Smooth fade (feels nicer than linear)
            t = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
