using System.Collections.Generic;
using UnityEngine;
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
