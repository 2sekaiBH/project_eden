using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem.XR.Haptics;

public class TurnFlowManager : MonoBehaviour
{
    // ----------------------------------------
    // 진행 상태 변수
    // ----------------------------------------
    private int currentTurn;
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
        TurnStart,
        DrawCards,
        PlayerSelect,
        OpponentSelect,
        ResolveTurn,
        TurnEnd,
    }
    public FlowState State { get; private set; } = FlowState.None;

    // ----------------------------------------
    // 이벤트
    // ----------------------------------------
    public event Action<int> OnTurnStart; // 턴수
    public event Action OnDrawCards;
    public event Action OnPlayerSubmitPhaseStart;

    [Header("Reference")]
    [SerializeField] PlayerActor playerActor;
    [SerializeField] OpponentActor opponentActor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerActor.HandManager.OnSelectEnd += HandlePlayerCardSubmit;
        opponentActor.OnOpponentEndSelect += HandleOpponentCardSubmit;

        InitializeState();
    }

    private void OnDisable()
    {
        playerActor.HandManager.OnSelectEnd -= HandlePlayerCardSubmit;
        opponentActor.OnOpponentEndSelect -= HandleOpponentCardSubmit;
    }

    public IEnumerator RunTurn()
    {
        InitializeState();

        // 1. 턴 시작
        currentTurn++;
        currentState = FlowState.TurnStart;

        playerActor.Initialize();
        opponentActor.Initialize();
        OnTurnStart?.Invoke(currentTurn);

        // 2. 카드덱에서 각자 카드를 뽑음
        currentState = FlowState.DrawCards;
        playerActor.DrawCards(5);
        opponentActor.DrawCards(5);
        OnDrawCards?.Invoke(); // 손패 UI 반영
        Debug.Log("랜덤 카드 드로우 완료");

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

        // 5. 점수 계산 및 턴 결과 반영

        yield return null;
    }

    private void HandlePlayerCardSubmit(List<CardData> pickedCard)
    {
        playerSelectedCards.AddRange(pickedCard);
        isPlayerSubmitted = true;
    }

    private void HandleOpponentCardSubmit(List<CardData> pickedCard)
    {
        opponentSelectedCards.AddRange(pickedCard);
        isOpponentSubmitted = true;
    }

    private void InitializeState()
    {
        currentState = FlowState.None;
        isPlayerSubmitted = false;
        isOpponentSubmitted = false;
        playerSelectedCards.Clear();
        opponentSelectedCards.Clear();
    }

}
