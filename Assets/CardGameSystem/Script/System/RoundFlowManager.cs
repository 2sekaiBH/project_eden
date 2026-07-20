using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RoundFlowManager : MonoBehaviour
{
    [Header("Round Setting")]
    [SerializeField] private int maxRounds = 5;
    private TurnFlowManager turnFlowManager;
    private Coroutine coRunRound; // 중복 검사 로직 추가

    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI roundTextUI;

    // ----------------------------------------
    // 진행 상태 변수
    // ----------------------------------------
    private int currentRound = 0;
    public int CurrentRound => currentRound;
    private bool turnResult = false; // 턴의 결과를 반영하는 플래그

    // ----------------------------------------
    // 이벤트
    // ----------------------------------------
    public static event Action<int> OnRoundStart; // 라운드 수
    public static event Action<int> OnRoundEnd;
    public static event Action <bool>OnResultDetermined; // 게임의 승패 판정을 알리는 이벤트 true - win, flase - lose  

    private void OnDisable()
    {
        turnFlowManager.OnPlayerWin -= HandlePlayerWin;
    }

    private void Awake()
    {
        turnFlowManager = GetComponent<TurnFlowManager>();
        turnFlowManager.OnPlayerWin += HandlePlayerWin;
    }

    void Start()
    {
        coRunRound= StartCoroutine(RunRound()); // 디버깅용
    }
    
    public IEnumerator RunRound()
    {
        Initialize(); // 라운드 상태 초기화
        while (currentRound < maxRounds)
        {
            // 1. 라운드 시작
            currentRound++;
            Debug.Log($"{currentRound}라운드 시작");
            UpdateUI(); // UI 반영
            OnRoundStart?.Invoke(currentRound);

            // 2. 턴 시작
            yield return StartCoroutine(turnFlowManager.RunTurn());

            // 3. 턴 종료
            if (turnResult == true)
            {
                OnResultDetermined?.Invoke(false); // 게임 매니저에서 처리
                yield break;
            }

            // 4. 라운드 종료
            OnRoundEnd?.Invoke(currentRound);

        }

        // 5라운드 초과 시 패배
        Debug.Log("패배");
        OnResultDetermined?.Invoke(false);
            
        yield return null;
    }

   /// <summary>
   /// 플레이어 승리 이벤트 핸들러
   /// </summary>
    private void HandlePlayerWin()
    {
        turnResult = true;
    }

    /// <summary>
    /// 상태 초기화
    /// </summary>
    private void Initialize()
    {
        currentRound = 0;
        turnResult = false;
    }

    /// <summary>
    /// UI 업데이터
    /// </summary>
    private void UpdateUI()
    {
        roundTextUI.text = $"현재 라운드: {currentRound}";
    }
}
