using UnityEngine;
using UnityEngine.Video;

public class OpeningFlowController : MonoBehaviour
{
    [Header("Opening Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject openingVideoRoot;

    [Header("Dialogue")]
    [SerializeField] private IntroDialogueController dialogueController;

    private bool hasFinished;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            Debug.LogError(
                "OpeningFlowController: VideoPlayer가 연결되지 않았습니다."
            );
            enabled = false;
            return;
        }

        if (openingVideoRoot == null)
        {
            Debug.LogError(
                "OpeningFlowController: OpeningVideoRoot가 연결되지 않았습니다."
            );
            enabled = false;
            return;
        }

        if (dialogueController == null)
        {
            Debug.LogError(
                "OpeningFlowController: DialogueController가 연결되지 않았습니다."
            );
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.errorReceived += OnVideoError;
    }

    private void OnDisable()
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.prepareCompleted -= OnVideoPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.errorReceived -= OnVideoError;
    }

    private void Start()
    {
        openingVideoRoot.SetActive(true);

        videoPlayer.isLooping = false;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        StartPrologue();
    }

    private void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogError($"오프닝 영상 재생 오류: {message}");

        // 영상 오류가 나도 게임 진행이 막히지 않도록 프롤로그로 넘어감
        StartPrologue();
    }

    public void SkipOpening()
    {
        StartPrologue();
    }

    private void StartPrologue()
    {
        if (hasFinished)
        {
            return;
        }

        hasFinished = true;

        videoPlayer.Stop();
        openingVideoRoot.SetActive(false);

        dialogueController.StartDialogueFromBeginning();
    }
}