using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class VideoToDialogue : MonoBehaviour
{
    [Header("Video and Dialogue")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private DialogueRunner dialogueRunner;

    [Header("Video Source")]
    [Tooltip("Filename inside Assets/StreamingAssets. Must match exactly, including extension.")]
    [SerializeField] private string videoFileName = "Cinematic.mp4";
    [Tooltip("If the video hasn't started within this many seconds, skip to dialogue.")]
    [SerializeField] private float prepareTimeout = 15f;

    [Header("Fade Function")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private float fadeInDuration = 5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI;

    [Header("Dialogue Node")]
    [SerializeField] private string startingNode = "Beginning";

    [Header("Skip Button")]
    [Tooltip("The skip button object. Hidden until the video is actually playing.")]
    [SerializeField] private GameObject skipButton;
    [SerializeField] private float skipButtonDelay = 3f;
    [SerializeField] private float skipFadeDuration = 0.4f;

    private bool videoStarted = false;
    private bool sequenceStarted = false;

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

        if (skipButton != null)
        {
            skipButton.SetActive(false);
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

            // WebGL cannot use VideoClip assets. Play from StreamingAssets over URL,
            // which also works in the editor and in desktop builds.
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFileName;

            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.errorReceived += OnVideoError;
            videoPlayer.Prepare();

            StartCoroutine(PrepareTimeoutWatchdog());
        }
        else
        {
            // No video player wired up at all: player moves forward to dialogue immediately.
            BeginStoryboardSequence();
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;

        videoStarted = true;
        videoPlayer.Play();
        StartCoroutine(WaitForVideoPlayback());
        StartCoroutine(ShowSkipButtonAfterDelay());
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("Cinematic video error: " + message + " (url: " + vp.url + ")");
        BeginStoryboardSequence();
    }

    private IEnumerator PrepareTimeoutWatchdog()
    {
        yield return new WaitForSeconds(prepareTimeout);

        if (!videoStarted)
        {
            Debug.LogWarning("Cinematic did not start within " + prepareTimeout
                + "s. Skipping to dialogue.");
            BeginStoryboardSequence();
        }
    }

    private IEnumerator ShowSkipButtonAfterDelay()
    {
        yield return new WaitForSeconds(skipButtonDelay);

        if (!sequenceStarted && skipButton != null)
        {
            skipButton.SetActive(true);
        }
    }

    // Public so the skip button's OnClick can call it from the inspector.
    public void SkipCinematic()
    {
        // Stop first so the audio cuts immediately rather than playing under the fade.
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        BeginStoryboardSequence(true);
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
            videoPlayer.errorReceived -= OnVideoError;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        BeginStoryboardSequence();
    }

    // Guarded so the handoff to dialogue can only ever happen once, no matter
    // whether it was triggered by the video finishing, an error, or the watchdog.
    private void BeginStoryboardSequence(bool skipped = false)
    {
        if (sequenceStarted)
        {
            return;
        }

        sequenceStarted = true;

        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }

        float outDuration = skipped ? skipFadeDuration : fadeOutDuration;
        float inDuration = skipped ? skipFadeDuration : fadeInDuration;

        StartCoroutine(ExecuteStoryboardSequence(outDuration, inDuration));
    }

    private IEnumerator ExecuteStoryboardSequence(float outDuration, float inDuration)
    {
        yield return StartCoroutine(Fade(0f, 1f, outDuration));

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false);
        }

        yield return StartCoroutine(Fade(1f, 0f, inDuration));

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