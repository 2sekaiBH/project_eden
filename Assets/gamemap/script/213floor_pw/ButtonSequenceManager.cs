using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ButtonSequenceManager : MonoBehaviour
{
    public static ButtonSequenceManager Instance { get; private set; }

    [Header("UI 연결")]
    [SerializeField] private GameObject sequencePanel;

    [Header("정답 순서 설정")]
    [SerializeField] private List<int> correctSequence = new List<int> { 3, 1, 4, 2 };

    [Header("이동할 다음 씬 이름")]
    [SerializeField] private string nextSceneName = "CardGameScene";

    [Header("성공 후 처리")]
    [SerializeField] private bool autoLoadNextScene = true;
    [SerializeField] private UnityEvent onSequenceSuccess;

    [Header("버튼 및 스프라이트 연동 (Button ID : 1, 2, 3, 4...)")]
    [SerializeField] private List<Image> buttonImages = new List<Image>();

    [Tooltip("기본 이미지 목록 (버튼 ID 순서대로)")]
    [SerializeField] private List<Sprite> defaultSprites = new List<Sprite>();

    [Tooltip("변경될(활성화/반전될) 이미지 목록 (버튼 ID 순서대로)")]
    [SerializeField] private List<Sprite> changedSprites = new List<Sprite>();

    // 플레이어가 지금까지 누른 버튼 ID들을 기록하는 리스트
    private List<int> playerSequence = new List<int>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if (sequencePanel != null) sequencePanel.SetActive(false);
    }
    public void OpenSequenceUI()
    {
        playerSequence.Clear(); // 입력 기록 리셋
        ResetAllButtonImages();
        if (sequencePanel != null) sequencePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseSequenceUI()
    {
        if (sequencePanel != null) sequencePanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public void OnPressButton(int buttonId)
    {
        playerSequence.Add(buttonId);
        int currentIndex = playerSequence.Count - 1; // 방금 누른 버튼의 순서(인덱스)

        Debug.Log($"버튼 {buttonId}번 클릭됨! (현재 입력 단계: {playerSequence.Count}/{correctSequence.Count})");

        ApplyButtonImageRules(buttonId);

        // 1. 방금 누른 버튼이 정답 순서의 해당 위치 번호와 일치하는지 검사
        if (playerSequence[currentIndex] != correctSequence[currentIndex])
        {
            Debug.Log("순서가 틀렸습니다! 처음부터 다시 누르세요.");
            OnSequenceFailed();
            return;
        }

        // 2. 정답
        if (playerSequence.Count == correctSequence.Count)
        {
            Debug.Log("모든 순서 일치! 다음 씬으로 이동합니다.");
            OnSequenceSuccess();
        }
    }

    private void ApplyButtonImageRules(int pressedButtonId)
    {
        switch (pressedButtonId)
        {
            case 1:
                ResetAllButtonImages();
                ChangeButtonImage(2, true);
                ChangeButtonImage(4, true);
                ChangeButtonImage(7, true);
                break;

            case 2:
                ResetAllButtonImages();
                ChangeButtonImage(3, true);
                ChangeButtonImage(5, true);
                ChangeButtonImage(8, true);
                break;

            case 3:
                ResetAllButtonImages();
                ChangeButtonImage(4, false);
                break;

            case 4:
                ResetAllButtonImages();
                ChangeButtonImage(5, true);
                ChangeButtonImage(8, true);
                break;

            case 5:
                ResetAllButtonImages();
                ChangeButtonImage(6, true);
                ChangeButtonImage(7, true);
                break;
                
            case 6:
                ResetAllButtonImages();
                ChangeButtonImage(7, true);
                ChangeButtonImage(10, true);
                break;

            case 7:
                ResetAllButtonImages();
                ChangeButtonImage(8, true);
                break;
            case 8:
                ResetAllButtonImages();
                ChangeButtonImage(9, true);
                break;
            case 9:
                ResetAllButtonImages();
                ChangeButtonImage(10, true);
                break;
        }
    }



        public void ChangeButtonImage(int targetButtonId, bool isChanged)
    {
        int index = targetButtonId - 1; // 버튼 ID가 1부터 시작하면 인덱스는 0부터이므로 -1

        if (index >= 0 && index < buttonImages.Count)
        {
            if (buttonImages[index] != null)
            {
                if (isChanged && index < changedSprites.Count && changedSprites[index] != null)
                {
                    buttonImages[index].sprite = changedSprites[index];
                }
                else if (!isChanged && index < defaultSprites.Count && defaultSprites[index] != null)
                {
                    buttonImages[index].sprite = defaultSprites[index];
                }
            }
        }
    }

    private void ResetAllButtonImages()
    {
        for (int i = 0; i < buttonImages.Count; i++)
        {
            if (buttonImages[i] != null && i < defaultSprites.Count && defaultSprites[i] != null)
            {
                buttonImages[i].sprite = defaultSprites[i];
            }
        }
    }

    // 실패했을 때 팝업 종료
    private void OnSequenceFailed()
    {
        CloseSequenceUI();
    }

    // 성공했을 때 씬 전환 로직
    private void OnSequenceSuccess()
    {
        Debug.Log("퍼즐 성공");

        CloseSequenceUI();

        // 퍼즐 성공 결과를 외부에 알림
        onSequenceSuccess?.Invoke();

        // 기존 방식이 필요한 경우에만 Scene 이동
        if (autoLoadNextScene && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
