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
  [SerializeField] private SkipModeController skipModeController;

  private bool isAuto = false;
  public bool IsAuto => isAuto;

  void Start() {
    UpdateVisuals();
  }

public void ToggleAuto() {
    isAuto = !isAuto;

    if (isAuto && skipModeController != null) skipModeController.ForceOff();

    if (linePresenter != null)
      linePresenter.autoAdvance = isAuto;

    UpdateVisuals();

    if (isAuto && dialogueRunner != null && dialogueRunner.IsDialogueRunning)
      dialogueRunner.RequestNextLine();
  }

  public void ForceOff() {
    if (!isAuto) return;
    isAuto = false;
    if (linePresenter != null) linePresenter.autoAdvance = false;
    UpdateVisuals();
  }

  private void UpdateVisuals() {
    if (autoButtonImage != null)
      autoButtonImage.sprite = isAuto ? autoOnSprite : autoOffSprite;

    bool skipIsOn = skipModeController != null && skipModeController.IsSkip;
    if (continueButton != null)
      continueButton.SetActive(!isAuto && !skipIsOn);
  }
}