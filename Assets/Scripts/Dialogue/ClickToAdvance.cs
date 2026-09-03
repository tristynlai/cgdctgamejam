using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;
using Yarn.Markup;
using Yarn.Unity;

public class ClickToAdvance : ActionMarkupHandler
{
  [SerializeField] private DialogueRunner dialogueRunner;
  private VisualNovel visualNovel;
  private bool lineIsFullyVisible = false;

  private void Awake()
  {
    visualNovel = FindObjectOfType<VisualNovel>();
  }

  private void Update()
  {
    bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
    bool mousePressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

    if (spacePressed || mousePressed)
    {
      Advance();
    }
  }

  public void Advance()
  {
    if (dialogueRunner == null || !dialogueRunner.IsDialogueRunning) { return; }

    if (visualNovel != null && visualNovel.IsDialoguePaused()) { return; }

    if (lineIsFullyVisible)
    {
      dialogueRunner.RequestNextLine();
    }
    else
    {
      dialogueRunner.RequestHurryUpLine();
    }
  }

  public override void OnPrepareForLine(MarkupParseResult line, TMP_Text text)
      => lineIsFullyVisible = false;

  public override void OnLineDisplayBegin(MarkupParseResult line, TMP_Text text)
      => lineIsFullyVisible = false;

  public override YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line, CancellationToken cancellationToken)
      => YarnTask.CompletedTask;

  public override void OnLineDisplayComplete()
      => lineIsFullyVisible = true;

  public override void OnLineWillDismiss()
      => lineIsFullyVisible = false;
}
