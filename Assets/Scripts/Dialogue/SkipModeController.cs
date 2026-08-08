using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;



public class SkipModeController : MonoBehaviour {

  [Header("Yarn" )]
  [SerializeField] private DialogueRunner dialogueRunner;
  [SerializeField] private LinePresenter linePresenter;

  [Header("Skip button visuals")]
[SerializeField] private Image skipButtonImage;
  [SerializeField] private Color skipOnTint = new Color(0.45f, 1f, 0.85f, 1f);

  [Header("Coordination")]
  [SerializeField] private AutoModeController autoModeController;
  [SerializeField] private GameObject continueButton;

  private bool isSkip = false;
    private Color skipOffTint = Color.white;
    private float originalAdvanceDelay;
    private bool originalUseFade;

    public bool IsSkip => isSkip;

    void Start() {
    if (skipButtonImage != null) skipOffTint = skipButtonImage.color;
    if (linePresenter != null) {
      originalAdvanceDelay = linePresenter.autoAdvanceDelay;
      originalUseFade = linePresenter.useFadeEffect;
    }
    UpdateVisuals();
  }

  void Update() {
    if (!isSkip) return;

    if (dialogueRunner == null || !dialogueRunner.IsDialogueRunning) {
      ForceOff();
      return;
    }

    dialogueRunner.RequestHurryUpLine();
  }

  public void ToggleSkip() => SetSkip(!isSkip);

  public void ForceOff() {
    if (isSkip) SetSkip(false);
  }

  private void SetSkip(bool on) {
    isSkip = on;

    if (on && autoModeController != null) autoModeController.ForceOff();

    if (linePresenter != null) {
      linePresenter.autoAdvance      = on;
      linePresenter.autoAdvanceDelay = on ? 0f : originalAdvanceDelay;
      linePresenter.useFadeEffect    = on ? false : originalUseFade;
    }

    UpdateVisuals();

    if (on && dialogueRunner != null && dialogueRunner.IsDialogueRunning)
      dialogueRunner.RequestNextLine();
  }

  private void UpdateVisuals() {
    if (skipButtonImage != null)
      skipButtonImage.color = isSkip ? skipOnTint : skipOffTint;

    bool autoIsOn = autoModeController != null && autoModeController.IsAuto;
    if (continueButton != null)
      continueButton.SetActive(!isSkip && !autoIsOn);
  }
}
