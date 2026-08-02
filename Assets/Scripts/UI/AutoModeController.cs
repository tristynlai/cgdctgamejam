using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class AutoModeController : MonoBehaviour {

  [Header("Yarn")]
  [SerializeField] private DialogueRunner dialogueRunner;
  [SerializeField] private LinePresenter linePresenter;

  [Header("Auto button visuals")]
  [SerializeField] private Image autoButtonImage;
  [SerializeField] private Sprite autoOnSprite;
  [SerializeField] private Sprite autoOffSprite;

  [Header("Continue button (hidden when auto is on)")]
  [SerializeField] private GameObject continueButton;

  private bool isAuto = false;

  void Start() {
    UpdateVisuals();
  }

  public void ToggleAuto() {
    isAuto = !isAuto;

    if (linePresenter != null)
      linePresenter.autoAdvance = isAuto;

    UpdateVisuals();

    if (isAuto && dialogueRunner != null && dialogueRunner.IsDialogueRunning)
      dialogueRunner.RequestNextLine();
  }

  private void UpdateVisuals() {
    if (autoButtonImage != null) {
      autoButtonImage.sprite = isAuto ? autoOnSprite : autoOffSprite;
    }

    if (continueButton != null) {
      continueButton.SetActive(!isAuto);
    }
  }
}