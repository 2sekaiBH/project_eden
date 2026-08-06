using UnityEngine;
using UnityEngine.Video;

public class ItemVideoUI : MonoBehaviour
{
    public static ItemVideoUI Instance { get; private set; }

    [Header("UI 및 연동 컴포넌트")]
    [SerializeField] private GameObject videoCanvasPanel;
    [SerializeField] private VideoPlayer videoPlayer;

    private int currentPendingItemId = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (videoCanvasPanel != null)
        {
            videoCanvasPanel.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    public void PlayRareItemVideo(int itemId)
    {
        currentPendingItemId = itemId;

        if (videoCanvasPanel != null)
        {
            videoCanvasPanel.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // 1. 동영상 UI 패널 끄기
        if (videoCanvasPanel != null)
        {
            videoCanvasPanel.SetActive(false);
        }

        // 2. 아이템 획득 팝업 UI 띄우기!
        if (currentPendingItemId != -1 && ItemAcquisitionUI.Instance != null)
        {
            ItemAcquisitionUI.Instance.ShowAcquisitionPopup(currentPendingItemId);
            currentPendingItemId = -1; // 초기화
        }
    }
}
