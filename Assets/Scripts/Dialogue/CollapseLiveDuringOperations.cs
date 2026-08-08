using UnityEngine;
using TMPro;

/// <summary>
/// Workaround for Unity's VerticalLayoutGroup reserving space for empty text.
/// Since Yarn requires the GameObject to remain active, this script watches the 
/// Options CanvasGroup and only disables the TextMeshPro component when choices 
/// appear, collapsing the phantom layout gap without breaking Yarn's logic.
/// </summary>

public class CollapseLiveLineDuringOptions : MonoBehaviour
{
    [SerializeField] CanvasGroup optionsCanvasGroup;  
    [SerializeField] TextMeshProUGUI liveLine;     

    bool optionsWereVisible;

    void Update()
    {
        if (optionsCanvasGroup == null || liveLine == null) return;

        bool optionsVisible = optionsCanvasGroup.alpha > 0.01f;
        if (optionsVisible == optionsWereVisible) return;

        optionsWereVisible = optionsVisible;
        liveLine.enabled = !optionsVisible;
    }
}