using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

public class TutorialHighlighter : MonoBehaviour
{
    private static TutorialHighlighter instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject messagesButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject historyButton;
    [SerializeField] private GameObject minigameButton;
    [SerializeField] private GameObject exitButton;

    private Coroutine activeGlowCoroutine;
    private GameObject currentlyHighlighted;
    private Dictionary<Image, Color> originalColors = new Dictionary<Image, Color>();

    private void Awake()
    {
        instance = this;
    }

    [YarnCommand("highlight_tab")]
    public static void HighlightTabCommand(string tabName)
    {
        if (instance != null)
        {
            instance.HighlightTab(tabName);
        }
    }

    [YarnCommand("clear_highlights")]
    public static void ClearHighlightsCommand()
    {
        if (instance != null)
        {
            instance.StopAllGlows();
        }
    }

    public void HighlightTab(string tabName)
    {
        StopAllGlows();

        GameObject target = tabName.ToLower() switch
        {
            "messages" => messagesButton,
            "settings" => settingsButton,
            "history" => historyButton,
            "minigame" => minigameButton,
            "exit" => exitButton,
            _ => null
        };

        if (target != null)
        {
            if (target == messagesButton || target == settingsButton)
            {
                target.SetActive(true);
            }

            currentlyHighlighted = target;
            activeGlowCoroutine = StartCoroutine(PulseGlowAndBrighten(target));
        }
    }

    public void StopAllGlows()
    {
        if (activeGlowCoroutine != null)
        {
            StopCoroutine(activeGlowCoroutine);
            activeGlowCoroutine = null;
        }

        foreach (var kvp in originalColors)
        {
            if (kvp.Key != null)
            {
                kvp.Key.color = kvp.Value;
            }
        }
        originalColors.Clear();

        if (currentlyHighlighted != null)
        {
            currentlyHighlighted.transform.localScale = Vector3.one;

            if (currentlyHighlighted == messagesButton || currentlyHighlighted == settingsButton)
            {
                currentlyHighlighted.SetActive(false);
            }

            currentlyHighlighted = null;
        }
    }

    private IEnumerator PulseGlowAndBrighten(GameObject obj)
    {
        yield return null;

        Transform t = obj.transform;
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 1.1f;

        Image[] images = obj.GetComponentsInChildren<Image>();
        originalColors.Clear();
        foreach (var img in images)
        {
            originalColors[img] = img.color;
        }

        while (true)
        {
            float elapsed = 0f;
            while (elapsed < 0.4f)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / 0.4f;

                t.localScale = Vector3.Lerp(originalScale, targetScale, progress);
                
                foreach (var kvp in originalColors)
                {
                    if (kvp.Key != null)
                    {
                        kvp.Key.color = Color.Lerp(kvp.Value, kvp.Value + new Color(0.3f, 0.3f, 0.3f, 0f), progress);
                    }
                }
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < 0.4f)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / 0.4f;

                t.localScale = Vector3.Lerp(targetScale, originalScale, progress);
                
                foreach (var kvp in originalColors)
                {
                    if (kvp.Key != null)
                    {
                        kvp.Key.color = Color.Lerp(kvp.Value + new Color(0.3f, 0.3f, 0.3f, 0f), kvp.Value, progress);
                    }
                }
                yield return null;
            }
        }
    }
}
