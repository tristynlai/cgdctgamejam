using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

#nullable enable

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class UseSizeOfText : MonoBehaviour, ILayoutElement
{
    private TMP_Text? Text => GetComponentInChildren<TMP_Text>();

    private RectTransform RectTransform => GetComponent<RectTransform>();

    float ILayoutElement.preferredHeight => minHeight;

    float ILayoutElement.preferredWidth => minWidth;

    float ILayoutElement.flexibleWidth => 0f;

    float ILayoutElement.flexibleHeight => 0f;

    public float minWidth { get; private set; } = 0f;
    public float minHeight { get; private set; } = 0f;

    int ILayoutElement.layoutPriority => 0;

    [SerializeField] float minimumWidth = 100f;

    [SerializeField] float minimumHeight = 30f;

    protected void OnEnable()
    {
        if (Text != null)
        {
            Text.OnPreRenderText += UpdateLayout;
            UpdateLayout(Text.textInfo);
        }
    }

    protected void OnDisable()
    {
        if (Text != null)
        {
            Text.OnPreRenderText -= UpdateLayout;
        }
    }

    private void UpdateLayout(TMP_TextInfo? info)
    {
        if (info == null || info.textComponent == null || string.IsNullOrEmpty(info.textComponent.text))
        {
            minHeight = minimumHeight;
            minWidth = minimumWidth;
            return;
        }

        info.textComponent.textWrappingMode = TMPro.TextWrappingModes.Normal;
     
        var parentWidth = minimumWidth;
        var parentRect = RectTransform.parent?.GetComponent<RectTransform>();
        if (parentRect != null)
        {
            parentWidth = parentRect.rect.width;
        }

        var xMargin = info.textComponent.margin.x + info.textComponent.margin.z;

        var insetSize = parentWidth - xMargin;

        var size = info.textComponent.GetPreferredValues(info.textComponent.text, insetSize, float.MaxValue);

        minHeight = Mathf.Max(minimumHeight, size.y);
        minWidth = Mathf.Max(minimumWidth, size.x + 5);

        LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
    }

    void ILayoutElement.CalculateLayoutInputHorizontal() { }

    void ILayoutElement.CalculateLayoutInputVertical() { }

    public void OnValidate() => UpdateLayout(null);
}

