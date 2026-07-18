using System;
using System.Collections;
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

    public enum FlowState
    {
        None,
        TurnStart,
        DrawCards,
        PlayerSubmit,
        EnemySubmit,
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
    [SerializeField] Actor playerActor;
    [SerializeField] Actor opponentActor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public IEnumerator RunTurn()
    {
        // 1. 턴 시작 - 초기화
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

        // 3. 플레이어 카드 제출
        // 카드 제출 시스템 스크립트에서 따로 처리

        // 4. 적 카드 제출


        // 5. 점수 계산 및 턴 결과 반영

        yield return null;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
