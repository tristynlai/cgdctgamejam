using System.Collections;
using UnityEngine;

public class SaveModeController : MonoBehaviour
{
    [SerializeField] private GameObject confirmationImage;
    [SerializeField] private float displayDuration = 2f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (confirmationImage != null)
        {
            confirmationImage.SetActive(false);
        }
    }

    public void SaveGameAndConfirm()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        
        if (saveManager != null)
        {
            saveManager.SaveGame();
        }
        else
        {
            Debug.LogError("SaveManager not found in the scene!");
        }

        ShowConfirmation();
    }

    private void ShowConfirmation()
    {
        if (confirmationImage == null) return;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        confirmationImage.SetActive(true);
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        confirmationImage.SetActive(false);
        hideCoroutine = null;
    }
}