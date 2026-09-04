using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class VideoToDialogue : MonoBehaviour
{
    [Header("Video and Dialogue")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private DialogueRunner dialogueRunner;

    [Header("Fade Function")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeOutDuration = 2f; 
    [SerializeField] private float fadeInDuration = 5f; 

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI; 

    [Header("Dialogue Node")]
    [SerializeField] private string startingNode = "Beginning";

    private void Awake()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 1f; 
        }

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }

        if (videoPlayer != null && videoPlayer.targetTexture != null)
        {
            RenderTexture rt = videoPlayer.targetTexture;
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
        }
    }

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        
        videoPlayer.Play();
        StartCoroutine(WaitForVideoPlayback());
    }

    private IEnumerator WaitForVideoPlayback()
    {
        while (videoPlayer != null && !videoPlayer.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        if (fadeOverlay != null)
        {
            yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration)); 
        }
    }

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
            videoPlayer.prepareCompleted -= OnVideoPrepared;
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

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(true);
        }

        if (dialogueRunner != null && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(startingNode);
        }
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
