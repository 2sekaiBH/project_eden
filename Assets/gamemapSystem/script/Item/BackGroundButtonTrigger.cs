using UnityEngine;
using UnityEngine.UI;

public class BackgroundButtonTrigger : MonoBehaviour
{
    [Header("감시할 배경 설정")]
    [Tooltip("IntroDialogueController에서 사용 중인 배경 UI Image")]
    [SerializeField] private Image targetBackgroundImage;

    [Tooltip("버튼이 작동해야 하는 배경의 Sprite 이미지")]
    [SerializeField] private Sprite targetBackgroundSprite;

    [Header("버튼 컴포넌트 (비워두면 자동 감지)")]
    [SerializeField] private Button myButton;

    [Header("아이템 설정")]
    [SerializeField] private int itemId; // 획득할 아이템 ID
    [SerializeField] private int targetClickCount = 3; // 3번 클릭

    private int currentClickCount = 0;
    private bool isRewardGranted = false;
    private Sprite lastCheckedSprite;

    private void Awake()
    {
        if (myButton == null)
        {
            myButton = GetComponent<Button>();
        }
    }

    private void Update()
    {
        // 배경 이미지가 변경되었는지 실시간 감지
        if (targetBackgroundImage != null && targetBackgroundImage.sprite != lastCheckedSprite)
        {
            lastCheckedSprite = targetBackgroundImage.sprite;
            CheckBackground();
        }
    }

    private void CheckBackground()
    {
        if (myButton == null || targetBackgroundSprite == null) return;

        // 현재 배경 이미지가 지정된 특정 배경 이미지와 일치하는지 확인
        bool isCorrectBackground = (targetBackgroundImage.sprite == targetBackgroundSprite);

        // 일치할 때만 버튼을 누를 수 있도록 활성화 (불일치 시 클릭 불가)
        myButton.interactable = isCorrectBackground;
    }

    public void OnClickTransparentButton()
    {
        if (isRewardGranted) return;

        currentClickCount++;
        Debug.Log($"비밀 클릭 진행 중... ({currentClickCount}/{targetClickCount})");

        if (currentClickCount >= targetClickCount)
        {
            GrantReward();
        }
    }

    private void GrantReward()
    {
        isRewardGranted = true;
        Debug.Log("3회 클릭 성공! 아이템을 지급합니다.");

        if (ItemAcquisitionUI.Instance != null)
        {
            ItemAcquisitionUI.Instance.ShowAcquisitionPopup(itemId);
        }
        else
        {
            Debug.LogError("ItemAcquisitionUI.Instance를 찾을 수 없습니다!");
        }

        // 획득 후 버튼 완전히 비활성화
        gameObject.SetActive(false);
    }
}
