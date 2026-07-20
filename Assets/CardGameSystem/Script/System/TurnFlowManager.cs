using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem.XR.Haptics;

public class TurnFlowManager : MonoBehaviour
{
    [Header("Turn Setting")]
    [SerializeField] private int turnsPerRound = 2;

    [Header("Reference")]
    [SerializeField] PlayerActor playerActor;
    [SerializeField] OpponentActor opponentActor;
    [SerializeField] TextMeshProUGUI turnTextUI;

    // ----------------------------------------
    // 진행 상태 변수
    // ----------------------------------------
    private int currentTurn = 0;
    public int CurrentTurn => currentTurn;

    private FlowState currentState;

    // 플레이어 카드 제출 확인용 플래그
    private bool isPlayerSubmitted = false;

    // 적 카드 제출 확인용 플래그
    private bool isOpponentSubmitted = false;

    // 플레이어에서 제출한 카드(뽑은 카드)
    private List<CardData> playerSelectedCards = new List<CardData>();

    // 적에서 제출한 카드
    private List<CardData> opponentSelectedCards = new List<CardData>();

    public enum FlowState
    {
        None,
        DrawCards,
        TurnStart,
        PlayerSelect,
        OpponentSelect,
        ResolveTurn,
        TurnEnd,
    }
    public FlowState State { get; private set; } = FlowState.None;

    // ----------------------------------------
    // 이벤트
    // ----------------------------------------
    public static event Action<int> OnTurnStart; // 턴수
    public static event Action<int> OnTurnEnd;
    public event Action OnPlayerWin; // 플레이어 승리 이벤트 - RounFlowManager에서 구독

    // 이벤트 구독 및 상태 변수 초기화
    void Start()
    {
        playerActor.HandManager.OnSelectEnd += HandlePlayerCardSubmit;
        opponentActor.OnOpponentEndSelect += HandleOpponentCardSubmit;

        InitializeState();
    }

    // 이벤트 해제
    private void OnDisable()
    {
        playerActor.HandManager.OnSelectEnd -= HandlePlayerCardSubmit;
        opponentActor.OnOpponentEndSelect -= HandleOpponentCardSubmit;
    }

    /// <summary>
    /// 메인으로 실행되는 turnflow 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunTurn()
    {
        currentTurn = 0;

        // 1. 카드덱에서 각자 카드를 뽑음
        currentState = FlowState.DrawCards;
        playerActor.DrawCards(5);
        opponentActor.DrawCards(5);
        Debug.Log("랜덤 카드 드로우 완료");

        while (currentTurn < turnsPerRound)
        {
            // 2. 턴 시작
            currentTurn++;
            Debug.Log($"{currentTurn}턴 시작");
            currentState = FlowState.TurnStart;
            UpdateUI(); // UI 반영

            playerActor.Initialize(); // player 상태 초기화
            opponentActor.Initialize(); // opponent 상태 초기화
            DeckManager.Instance.InitializeDeck(); // deck 초기화
            OnTurnStart?.Invoke(currentTurn); 

            // 3. 플레이어 카드 제출
            currentState = FlowState.PlayerSelect;
            playerActor.SelectCard(); 
            yield return new WaitUntil(() => isPlayerSubmitted);
            Debug.Log("player가 낸 카드: " + string.Join(", ", playerSelectedCards.Select(p => p.name)));

            // 4. 적 카드 제출
            currentState = FlowState.OpponentSelect;
            opponentActor.SelectCard();
            yield return new WaitUntil(() => isOpponentSubmitted);
            Debug.Log("상대편이 낸 카드: " + string.Join(", ", opponentSelectedCards.Select(p => p.name)));

            // 5. 카드 실행

            // 6. 결과 계산
            
            // 7. 승리 판정
            if(opponentActor.CurrentHp <= 0)
            {
                Debug.Log("승리");
                OnPlayerWin?.Invoke();
                InitializeState();
                yield break; // turn 코루틴 종료
            }

            // 8. 턴 종료
            InitializeState(); // 상태 초기화
            OnTurnEnd?.Invoke(currentTurn);

            yield return null;
        }
    }
    /// <summary>
    /// 플레이어 제출이 완료됐을 때 실행되는 핸들러
    /// </summary>
    /// <param name="pickedCard">선택한 카드 리스트</param>
    private void HandlePlayerCardSubmit(List<CardData> pickedCard)
    {
        playerSelectedCards.AddRange(pickedCard);
        isPlayerSubmitted = true;
    }

    /// <summary>
    /// 적의 제출이 완료됐을 때 실행되는 핸들러
    /// </summary>
    /// <param name="pickedCard">선택한 카드 리스트</param>
    private void HandleOpponentCardSubmit(List<CardData> pickedCard)
    {
        opponentSelectedCards.AddRange(pickedCard);
        isOpponentSubmitted = true;
    }

    /// <summary>
    /// 상태 변수 초기화
    /// </summary>
    private void InitializeState()
    {
        currentState = FlowState.None;
        isPlayerSubmitted = false;
        isOpponentSubmitted = false;
        playerSelectedCards.Clear();
        opponentSelectedCards.Clear();
    }
    
    /// <summary>
    /// UI 업데이터
    /// </summary>
    private void UpdateUI()
    {
        turnTextUI.text = $"현재 턴: {currentTurn}";
    }
}
