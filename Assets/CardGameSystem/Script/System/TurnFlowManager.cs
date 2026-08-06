using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// turn의 흐름을 제어하는 스크립트
/// </summary>
public class TurnFlowManager : MonoBehaviour
{
    [Header("Turn Setting")]
    [SerializeField] private int turnsPerRound = 2;

    [Header("Reference")]
    [SerializeField] PlayerActor playerActor;
    [SerializeField] OpponentActor opponentActor;
    [SerializeField] CardExecutor cardExecutor;
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

    // 유저가 선택한 npc 슬롯
    private NpcData selectedNpcData;

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
        HandManager.OnSelectEnd += HandlePlayerCardSubmit;
        OpponentActor.OnOpponentEndSelect += HandleOpponentCardSubmit;

        InitializeState();


    }

    // 이벤트 해제
    private void OnDisable()
    {
        HandManager.OnSelectEnd -= HandlePlayerCardSubmit;
        OpponentActor.OnOpponentEndSelect -= HandleOpponentCardSubmit;
    }

    /// <summary>
    /// 메인으로 실행되는 turnflow 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunTurn()
    {
        currentTurn = 0;

        // 1. 카드덱에서 각자 카드를 뽑음
        DeckManager.Instance.InitializeDeck(); // deck 초기화
        currentState = FlowState.DrawCards;

        // 카드 뽑기
        playerActor.DrawCards(5);
        opponentActor.DrawCards(5);

        PendingEffectManager.Instance.ApplyRoundPendingState(playerActor, opponentActor); // 이전 턴에서 반영해야할 정보들 반영
        UIUpdator.Instance.SetText($"랜덤 카드 드로우 완료");
        Debug.Log("랜덤 카드 드로우 완료");
        yield return new WaitForSeconds(1f);

        while (currentTurn < turnsPerRound)
        {
            // 2. 턴 시작
            currentTurn++;
            currentState = FlowState.TurnStart;
            playerActor.EnergyIntialize(); // 플레이어 에너지 초기화
            opponentActor.EnergyIntialize(); // 적 에너지 초기화

            UpdateUI(); // Turn 정보 UI 갱신

            UIUpdator.Instance.SetText($"{currentTurn}턴 시작");
            Debug.Log($"{currentTurn}턴 시작");
            yield return new WaitForSeconds(1f);

            // 3. 평타 공격 - DefaultAttackController에서 담당
            OnTurnStart?.Invoke(currentTurn);
            UIUpdator.Instance.SetText($"평타 발동: <sprite=1>-2, <sprite=2>+1", CasterType.Player);
            yield return new WaitForSeconds(1f);

            PendingEffectManager.Instance.ConsumeReduceCost(); //카드 코스트 -1

            // 4. 플레이어 카드 제출, Npc 효과 처리
            currentState = FlowState.PlayerSelect;
            playerActor.SelectCard();
            yield return new WaitUntil(() => isPlayerSubmitted);
            Debug.Log("나의 카드 제출: " + string.Join(", ", playerSelectedCards.Select(p => p.name)));
            //UIUpdator.Instance.SetText($"나의 카드 제출: {string.Join(", ", playerSelectedCards.Select(p => p.name))}", CasterType.Player);

            GaugeManager.Instance.SameCardType(playerSelectedCards); //같은 종류의 카드만 사용했는지 확인
            GaugeManager.Instance.AllEnergy(playerActor); //이번 턴에 에너지를 다 썼는지 확인
            GaugeManager.Instance.UseAdaptive(playerSelectedCards); //조건부 카드를 사용했는지 확인
          
            // 5. 적 카드 제출
            currentState = FlowState.OpponentSelect;
            opponentActor.SelectCard();
            yield return new WaitUntil(() => isOpponentSubmitted);
            Debug.Log("상대 카드 제출: " + string.Join(", ", opponentSelectedCards.Select(p => p.name)));
            //UIUpdator.Instance.SetText($"상대 카드 제출: {string.Join(", ", opponentSelectedCards.Select(p => p.name))}", CasterType.Opponent);

            // 6. 카드 실행
            yield return cardExecutor.CardExecuteControll(playerActor, playerSelectedCards, opponentActor, opponentSelectedCards);


            //집중 게이지를 다 채웠을 경우의 공격 발동)
            if (GaugeManager.Instance.CurrentGauge >= GaugeManager.Instance.MaxGauge)
            {
                GaugeManager.Instance.Burst();
                opponentActor.TakeDamage(15, playerActor); //데미지 15를 가함
            }


            //Corruption 카드 효과 발동
            //카드 효과로 승리할 수 있으므로 판정 앞에 배치
            PendingEffectManager.Instance.ConsumeEndturnDamage();

            // 7. 승리 판정
            if (opponentActor.CurrentHp <= 0)
            {
                OnPlayerWin?.Invoke();
                InitializeState();
                UIUpdator.Instance.SetText($"승리");
                Debug.Log("승리");
                yield break; // turn 코루틴 종료
            }

            // 8. 턴 종료
            InitializeState(); // 상태 변수 초기화
            OnTurnEnd?.Invoke(currentTurn);
            playerActor.ResetTurnEffect(); //플레이어에게 적용되는 턴 지속 효과 초기화
            opponentActor.ResetTurnEffect(); //몹에게 적용되는 턴 지속 효과 초기화

            MissionManager.Instance.EvaluateMission(); //최종적으로 턴이 끝나고 미션 조건을 충족했는지 확인

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
        foreach (CardData card in pickedCard) //적이 사용한 카드 삭제
        {
            opponentActor.DiscardCard(card);
        }

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
        turnTextUI.text = $"{currentTurn} 턴";
    }
}