using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

public class WirePuzzleController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public GameObject puzzleCanvas;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text barkText;
    [SerializeField] private TMP_Text lampText;
    [SerializeField] private Image panelBackgroundRenderer;
    [SerializeField] private Image statusContainerImage;
    [SerializeField] private Image lampSpriteImage;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject recallButton;
    [SerializeField] private Transform wireContainer;

    [Header("Panel State Sprites")]
    [SerializeField] private Sprite panelDefault;
    [SerializeField] private Sprite panelError;
    [SerializeField] private Sprite panelFail;
    [SerializeField] private Sprite panelWin;

    [Header("Status Bar Sprites")]
    [SerializeField] private Sprite statusDefaultSprite;
    [SerializeField] private Sprite statusErrorSprite;
    [SerializeField] private Sprite statusFailSprite;
    [SerializeField] private Sprite statusWinSprite;

    [Header("Lamp Sprites")]
    [SerializeField] private Sprite redLampSprite;
    [SerializeField] private Sprite greenLampSprite;

    [Header("Yarn Things")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private VisualNovel visualNovel;

    private readonly List<string> correctSequence = new List<string> { "Red", "Blue", "Yellow", "Green" };
    private int currentStep = 0;
    private int failCount = 0;
    private bool isPuzzleActive = false;
    private bool isProcessingError = false;
    private bool isFinished = false;

    private Color defaultTextColor;

    private void Awake()
    {
        if (visualNovel == null)
        {
            visualNovel = FindObjectOfType<VisualNovel>();
        }
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
        }

        if (statusText != null)
        {
            defaultTextColor = statusText.color;
        }

        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false);
        }
    }

    [YarnCommand("wait_for_wire_puzzle")]
    public static IEnumerator RegisterPuzzleCommand()
    {
        var controller = FindObjectOfType<WirePuzzleController>(true);
        if (controller != null)
        {
            controller.StartPuzzle();

            while (controller.isPuzzleActive)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogError("WirePuzzleController not found!");
        }
    }

    public void StartPuzzle()
    {
        currentStep = 0;
        failCount = 0;
        isPuzzleActive = true;
        isProcessingError = false;
        isFinished = false;

        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }
        if (recallButton != null)
        {
            recallButton.SetActive(true);
        }

        SetState("SYSTEM: LOCKED - 3 ATTEMPTS REMAINING", panelDefault, statusDefaultSprite, defaultTextColor);
        SetLampState("LOCKED", redLampSprite);
        SetBark("It’s definitely some sort of code…");
        ResetAllWires();
        
        if (visualNovel != null)
        {
            visualNovel.PauseDialogue(true);
        }
        SetDialogueInputActive(false);

        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(true);
        }
    }

    public void OnWireClicked(string wireColor)
    {
        if (dialogueRunner == null || isProcessingError || isFinished) return;

        if (correctSequence[currentStep] == wireColor)
        {
            SetWireLit(wireColor, true);

            if (currentStep == 0) SetBark("Now we’re getting somewhere.");
            else if (currentStep == 1) SetBark("Ok getting closer….");
            else if (currentStep == 2) SetBark("Come on... so close");
            else if (currentStep == 3) SetBark("YES! The flux ring drops from the gull’s neck and he flies away");

            currentStep++;
            //Debug.Log($"Correct wire clicked: {wireColor}. Step {currentStep}/4");

            if (currentStep >= correctSequence.Count)
            {
                SetLampState("UNLOCKED", greenLampSprite);
                TriggerFinishState(true, true, "$puzzleSolved", true, "$hasFluxRing", true, "$gullOwesLuna", true, "ACCESS GRANTED", panelWin, statusWinSprite, Color.green);
            }
        }
        else
        {
            failCount++;
            dialogueRunner.VariableStorage.SetValue("$wire_fails", failCount);

            //Debug.Log($"Wrong wire clicked: {wireColor}. Fail count: {failCount}");

            if (failCount == 1)
            {
                SetBark("The cybergull is screeching in pain... I need to be more careful");
                StartCoroutine(HandleErrorSequence("ERROR: TWO MORE ATTEMPTS", "SYSTEM: LOCKED - 2 ATTEMPTS REMAINING", panelError, statusErrorSprite));
            }
            else if (failCount == 2)
            {
                SetBark("The cybergull is freaking out... I think Val mentioned something about flux rings when we were talking...");
                StartCoroutine(HandleErrorSequence("ERROR:ONE MORE ATTEMPT", "SYSTEM: LOCKED - 1 ATTEMPTS REMAINING", panelError, statusErrorSprite));
            }
            else if (failCount >= 3)
            {
                SetBark("Damn, I missed my chance to help him. The cybergull panics and scrambles away from me into the dark…");
                SetLampState("LOCKED DOWN", redLampSprite);
                TriggerFinishState(false, false, "$puzzleSolved", false, "$gullOwesLuna", false, null, false, "SEQUENCE FAILED: SYSTEM LOCKDOWN", panelFail, statusFailSprite, Color.red);
            }
        }
    }

    private void SetBark(string message)
    {
        if (barkText != null)
        {
            barkText.text = message;
        }
    }

    private void SetLampState(string labelText, Sprite lampSprite)
    {
        if (lampText != null)
        {
            lampText.text = labelText;
        }
        if (lampSpriteImage != null && lampSprite != null)
        {
            lampSpriteImage.sprite = lampSprite;
        }
    }

    private void TriggerFinishState(bool puzzleSolvedVal, bool hasFluxVal, string var1Name, bool var1Val, string var2Name, bool var2Val, string var3Name, bool var3Val, string statusMsg, Sprite panelSprite, Sprite statusSprite, Color textColor)
    {
        isFinished = true;
        
        if (!string.IsNullOrEmpty(var1Name)) dialogueRunner.VariableStorage.SetValue(var1Name, var1Val);
        if (!string.IsNullOrEmpty(var2Name)) dialogueRunner.VariableStorage.SetValue(var2Name, var2Val);
        if (!string.IsNullOrEmpty(var3Name)) dialogueRunner.VariableStorage.SetValue(var3Name, var3Val);

        SetState(statusMsg, panelSprite, statusSprite, textColor);

        if (recallButton != null)
        {
            recallButton.SetActive(false);
        }
        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }
    }

    public void OnContinueButtonClicked()
    {
        if (!isFinished) return;

        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false);
        }

        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }
        if (recallButton != null)
        {
            recallButton.SetActive(true);
        }

        var fadeGroups = FindObjectsOfType<CanvasGroup>();
        foreach (var group in fadeGroups)
        {
            if (group.gameObject.name.Contains("Fade") || group.gameObject.name.Contains("Black"))
            {
                group.alpha = 0f;
            }
        }

        if (visualNovel != null)
        {
            visualNovel.PauseDialogue(false);
        }
        SetDialogueInputActive(true);

        isPuzzleActive = false;
    }

    private void SetWireLit(string colorName, bool isLit)
    {
        if (wireContainer == null) return;

        string targetObjectName = "Wire_" + colorName.ToLower();
        Transform wireObj = wireContainer.Find(targetObjectName);

        if (wireObj == null)
        {
            foreach (Transform child in wireContainer)
            {
                if (child.name.Equals(targetObjectName, System.StringComparison.OrdinalIgnoreCase))
                {
                    wireObj = child;
                    break;
                }
            }
        }

        if (wireObj != null)
        {
            Transform litChild = wireObj.Find("Lit");
            if (litChild != null)
            {
                litChild.gameObject.SetActive(isLit);
            }
        }
    }

    private void ResetAllWires()
    {
        if (wireContainer == null) return;

        foreach (Transform wireObj in wireContainer)
        {
            Transform litChild = wireObj.Find("Lit");
            if (litChild != null)
            {
                litChild.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator HandleErrorSequence(string errorMsg, string nextCountdownMsg, Sprite errorSprite, Sprite statusErrorSp)
    {
        isProcessingError = true;
        SetState(errorMsg, errorSprite, statusErrorSp, Color.red);
        
        yield return new WaitForSeconds(1.2f);

        SetState(nextCountdownMsg, panelDefault, statusDefaultSprite, defaultTextColor);
        isProcessingError = false;
    }

    private void SetState(string message, Sprite targetPanelSprite, Sprite targetStatusSprite, Color textColor)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = textColor;
        }
        if (panelBackgroundRenderer != null && targetPanelSprite != null)
        {
            panelBackgroundRenderer.sprite = targetPanelSprite;
        }
        if (statusContainerImage != null && targetStatusSprite != null)
        {
            statusContainerImage.sprite = targetStatusSprite;
        }
    }

    private void SetDialogueInputActive(bool active)
    {
        var advancers = FindObjectsOfType<ClickToAdvance>();
        foreach (var advancer in advancers)
        {
            advancer.enabled = active;
        }
    }
}
