using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

/// <summary>
/// round의 흐름을 제어하는 스크립트
/// </summary>
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
    private bool isTurnResultDetermined = false; // 턴의 결과를 반영하는 플래그
    private bool turnResult = false; // 턴의 승패 결과를 저장하는 플래그

    // ----------------------------------------
    // 이벤트
    // ----------------------------------------
    public static event Action<int> OnRoundStart; // 라운드 수
    public static event Action<int> OnRoundEnd;
    public event Action <bool>OnResultDetermined; // 게임의 승패 판정을 알리는 이벤트 true - win, flase - lose  


    private void OnDisable()
    {
        turnFlowManager.OnTurnResultDetermined -= HandleTurnResult;
    }

    private void Awake()
    {
        turnFlowManager = GetComponent<TurnFlowManager>();
        turnFlowManager.OnTurnResultDetermined += HandleTurnResult;

    }

    /// <summary>
    /// 라운드 시작 함수
    /// </summary>
    public void StartRound()
    {
        coRunRound = StartCoroutine(RunRound());
    }

    /// <summary>
    /// 메인 라운드 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunRound()
    {
        Initialize(); // 라운드 상태 초기화
        while (currentRound < maxRounds)
        {
            // 1. 라운드 시작
            currentRound++;
            MissionManager.Instance.GenerateMission(); //미션 생성
            UpdateUI(); // UI 반영

            OnRoundStart?.Invoke(currentRound);

            UIUpdator.Instance.SetText($"{currentRound}라운드 시작");
            Debug.Log($"{currentRound}라운드 시작");
            yield return new WaitForSeconds(1f);

            // 2. 턴 시작
            yield return StartCoroutine(turnFlowManager.RunTurn());

            // 3. 턴 종료
            if (isTurnResultDetermined == true)
            {
                yield return new WaitForSeconds(3f);
                OnResultDetermined?.Invoke(turnResult); // 게임 매니저에서 처리 - 승리
                yield break;
            }

            // 4. 라운드 종료
            OnRoundEnd?.Invoke(currentRound);
            UIUpdator.Instance.SetText($"{currentRound} 라운드 종료");
            Debug.Log($"{currentRound} 종료");
            yield return new WaitForSeconds(1f);
        }

        UIUpdator.Instance.SetText($"패배");
        Debug.Log($"패배");
        yield return new WaitForSeconds(3f);

        // 5라운드 초과 시 패배
        OnResultDetermined?.Invoke(false);

        yield return null;
    }

   /// <summary>
   /// 플레이어 승리 이벤트 핸들러
   /// </summary>
    private void HandleTurnResult(bool result)
    {
        isTurnResultDetermined = true;
        turnResult = result;
    }

    /// <summary>
    /// 상태 초기화
    /// </summary>
    private void Initialize()
    {
        currentRound = 0;
        isTurnResultDetermined = false;
    }

    /// <summary>
    /// UI 업데이터
    /// </summary>
    private void UpdateUI()
    {
        roundTextUI.text = $"{currentRound} 라운드";
    }
}
