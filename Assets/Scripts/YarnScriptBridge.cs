using UnityEngine;
using Yarn.Unity;

public class YarnScriptBridge : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public CyberdeckController cyberdeckController;

    void Start()
    {
        if (dialogueRunner != null && cyberdeckController != null)
        {
            dialogueRunner.AddCommandHandler<int>("set_cyberdeck_state", cyberdeckController.SetCyberdeckState);
        }
    }
}
