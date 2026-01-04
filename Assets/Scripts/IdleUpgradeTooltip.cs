using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class IdleUpgradeTooltip : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI tooltipText;
    public IdleUpgrade idleUpgrade;

    public float moveDistance = 10f;
    public float fadeDuration = 0.1f;

    RectTransform rectTransform;
    Vector3 hiddenPosition;
    Vector3 visiblePosition;

    Coroutine fadeCoroutine;

    void Start()
    {
        rectTransform = canvasGroup.GetComponent<RectTransform>();

        visiblePosition = rectTransform.localPosition;
        hiddenPosition = visiblePosition - Vector3.up * moveDistance;

        canvasGroup.alpha = 0f;
        rectTransform.localPosition = hiddenPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RefreshTooltip();
        StartFade(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(0f);
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvas(targetAlpha));
    }

    IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        Vector3 startPos = rectTransform.localPosition;

        Vector3 targetPos = targetAlpha > 0.5f
            ? visiblePosition
            : hiddenPosition;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        rectTransform.localPosition = targetPos;
    }

    public void RefreshTooltip()
    {
        tooltipText.text =
            $"{idleUpgrade.level + idleUpgrade.GetEffectivePointsPerSecond()}/s\n" +
            $"Custo: {idleUpgrade.cost}";

        Debug.Log(idleUpgrade.level + idleUpgrade.GetEffectivePointsPerSecond());
    }

}
