  using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Yarn.Unity;
using System.Reflection;

public class AutoModeController : MonoBehaviour {

  [Header("Yarn")]
  [SerializeField] private DialogueRunner dialogueRunner;

  [Header("Auto button visuals")]
  [SerializeField] private Image autoButtonImage;
  [SerializeField] private Sprite autoOnSprite;
  [SerializeField] private Sprite autoOffSprite;

  [Header("Continue button (hidden when auto is on)")]
  [SerializeField] private GameObject continueButton;

  [Header("Timing")]
  [SerializeField] private float autoAdvanceDelay = 2f;

  private bool isAuto = false;
  private Coroutine autoRoutine;

void Start() {

  UpdateVisuals();
  
  }
  
  // This is going to be wired to the AUTO button's OnClick!! 
  public void ToggleAuto() {
    isAuto = !isAuto;
    UpdateVisuals();

    if (isAuto) {
      autoRoutine = StartCoroutine(AutoAdvanceRoutine());
    } else if (autoRoutine != null) {
      StopCoroutine(autoRoutine);
      autoRoutine = null;
    }
  }

private void UpdateVisuals() {
  if (autoButtonImage != null) {
    autoButtonImage.sprite = isAuto ? autoOnSprite : autoOffSprite;
  }

  if (continueButton != null) {
    continueButton.SetActive(!isAuto);
  }
    
}

private IEnumerator AutoAdvanceRoutine() {
  while (isAuto) {
    yield return new WaitForSeconds(autoAdvanceDelay);
 if (isAuto && dialogueRunner != null && dialogueRunner.IsDialogueRunning) {
      dialogueRunner.RequestNextLine();
    }
  }
}
} 

