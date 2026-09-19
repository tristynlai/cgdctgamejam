using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WireButton : MonoBehaviour
{
    [SerializeField] private string wireColor;
    [SerializeField] private WirePuzzleController puzzleController;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickWire);

        if (puzzleController == null)
        {
            puzzleController = FindObjectOfType<WirePuzzleController>();
        }
    }

    private void OnClickWire()
    {
        if (puzzleController != null)
        {
            puzzleController.OnWireClicked(wireColor);
        }
    }
}
