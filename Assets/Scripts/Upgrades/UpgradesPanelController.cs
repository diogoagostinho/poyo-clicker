using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UpgradesPanelController : MonoBehaviour
{
    public RectTransform panel;
    public CanvasGroup canvasGroup;

    public float slideDuration = 0.2f;
    float panelWidth;

    bool isOpen;
    Coroutine slideCoroutine;


    void Start()
    {
        panelWidth = panel.rect.width;

        panel.anchoredPosition = new Vector2(-panelWidth, 0);
        canvasGroup.alpha = 0f;

        Canvas.ForceUpdateCanvases();
    }

    void Update()
    {
        if (!isOpen) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            bool clickedOnPanel = false;
            foreach (var result in results)
            {
                if (result.gameObject == panel.gameObject || result.gameObject.transform.IsChildOf(panel))
                {
                    clickedOnPanel = true;
                    break;
                }
            }

            if (!clickedOnPanel)
            {
                ClosePanel();
            }
        }
    }

    public void ClosePanel()
    {
        if (!isOpen) return;

        isOpen = false;

        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        slideCoroutine = StartCoroutine(SlidePanel(false));
    }

    public void TogglePanel()
    {
        Canvas.ForceUpdateCanvases();
        isOpen = !isOpen;

        if (isOpen)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(panel);
        }

        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        slideCoroutine = StartCoroutine(SlidePanel(isOpen));
    }

    IEnumerator SlidePanel(bool open)
    {
        float elapsed = 0f;

        Vector2 startPos = panel.anchoredPosition;
        Vector2 endPos = open
            ? Vector2.zero
            : new Vector2(-panelWidth, 0);

        float startAlpha = canvasGroup.alpha;
        float endAlpha = open ? 1f : 0f;

        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slideDuration;

            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        panel.anchoredPosition = endPos;
        canvasGroup.alpha = endAlpha;
    }
}
