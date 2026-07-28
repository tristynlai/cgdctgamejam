using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class VideoToDialogue : MonoBehaviour
{
    [Header("Video and Dialogue")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private DialogueRunner dialogueRunner;
    
    [Header("Fade Aspects")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeInDuration = 2.5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI; 

    [Header("Dialogue Node")]
    [SerializeField] private string startingNode = "Intro";

    private void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(ExecuteStoryboardSequence());
    }

    private IEnumerator ExecuteStoryboardSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration));

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false); 
        }

        yield return StartCoroutine(Fade(1f, 0f, fadeInDuration));

    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        if (fadeOverlay != null)
        {
            float time = 0;
            fadeOverlay.alpha = startAlpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                yield return null;
            }
            
            fadeOverlay.alpha = targetAlpha;
        }
    }
}