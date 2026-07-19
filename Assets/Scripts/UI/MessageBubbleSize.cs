using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class MessageBubbleSize : MonoBehaviour, ILayoutElement
{
    private TMP_Text Text => GetComponentInChildren<TMP_Text>();
    private RectTransform RectTransform => GetComponent<RectTransform>();

    float ILayoutElement.preferredHeight => minHeight;
    float ILayoutElement.preferredWidth => minWidth;

    float ILayoutElement.flexibleWidth => 0f;
    float ILayoutElement.flexibleHeight => 0f;

    public float minWidth { get; private set; } = 0f;
    public float minHeight { get; private set; } = 0f;

    int ILayoutElement.layoutPriority => 0;

    [SerializeField] float minimumWidth = 250f;
    [SerializeField] float minimumHeight = 30f;

    void ILayoutElement.CalculateLayoutInputHorizontal() { }
    void ILayoutElement.CalculateLayoutInputVertical() { }

    private void UpdateLayout(TMP_TextInfo info)
    {
        if (info == null || info.textComponent == null || string.IsNullOrEmpty(info.textComponent.text))
        {
            minHeight = minimumHeight;
            minWidth = minimumWidth;
            return;
        }

        // Calculate the maximum width available to us by getting our
        // parent's width
        var parentWidth = RectTransform.parent.GetComponent<RectTransform>().rect.width;

        // Get the left and right margins of the text component
        var xMargin = info.textComponent.margin.x + info.textComponent.margin.z;

        // Get the total width available for drawing text
        var insetSize = parentWidth - xMargin;

        // Compute the rectangle we'd need to draw the text in, given our
        // available width and an (effectively) unlimited amount of vertical
        // space
        var size = info.textComponent.GetPreferredValues(info.textComponent.text, insetSize, float.MaxValue);

        // Our minimum width and height are now based on this (we add a
        // slight padding to the width)
        minHeight = Mathf.Max(minimumHeight, size.y);
        minWidth = Mathf.Max(minimumWidth, size.x + 5);

        // Now that we know our minimum width and height, ask the layout
        // system to rebuild our layout
        LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
    }

    protected void OnValidate() => UpdateLayout(null);

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
}
