using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player의 손패 관리 매니저
/// PlayerActor와 CardDisplay 사이의 인터페이스 역할
/// </summary>
public class HandManager : MonoBehaviour
{
    [Header("Refernce")]
    [SerializeField] private List<GameObject> cards = new List<GameObject>(); // 카드 오브젝트들
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private PlayerActor player;
    private List<CardDisplay> cardDisplays = new List<CardDisplay>(); // 카드 오브젝트에 부착된 CardDisplay

    private List<CardData> selectedCards = new List<CardData>(); // 선택된 카드 리스트

    public List<CardData> affordableCards = new List<CardData>(); // 이번 턴에서 선택할 수 있는 카드 리스트
    public List<CardData> AffordableCards => affordableCards;

    private bool selectEndFlag = false; // 선택 종료 플래그
    private RectTransform rectTransform;   

    public void HandleSelectEndFlag(bool value) // 제출 버튼에서 구독
    {
        selectEndFlag = value;
    }

    public event Action<List<CardData>> OnSelectEnd;
    // 플레이어 선택 최종 종료 이벤트
    // PlayerActor에서 구독

    public static Action<int> OnCardSelect;
    // CardDisplay에서 구독

    private Action<int> _OnRoundEndHandler;

    private void Awake()
    {
        cards.ForEach((card) => cardDisplays.Add(card.GetComponent<CardDisplay>()));
        rectTransform = GetComponent<RectTransform>();
    }

    // 이벤트 구독
    private void OnEnable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected += HandleSelectCard);

        _OnRoundEndHandler = ((int _) => { FillCard(); ResetAffordableCards(); });
        RoundFlowManager.OnRoundEnd += _OnRoundEndHandler;
        player.OnPlayerDrawCard += OnActorDrawCardHandler;
    }
    // 이벤트 해제
    private void OnDisable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected -= HandleSelectCard);
        RoundFlowManager.OnRoundEnd -= _OnRoundEndHandler;
        player.OnPlayerDrawCard -= OnActorDrawCardHandler;
    }

    /// <summary>
    /// 손패 업데이트
    /// </summary>
    public void Initialize(List<CardData> cardDatas)
    {
        if (cardDatas.Count > cards.Count)
        {
            for (int i = 0; i < cardDatas.Count - cards.Count + 1; i++) // 5개보다 더 많은 손패 존재 시 카드 오브젝트 새로 생성
            {
                GameObject extraCard = Instantiate(cardPrefab, rectTransform, false);
                cards.Add(extraCard);
                CardDisplay extraDisplay = extraCard.GetComponent<CardDisplay>();
                cardDisplays.Add(extraDisplay);
                extraDisplay.OnCardSelected += HandleSelectCard;
            }
        }
        for (int i = 0; i < cardDisplays.Count; i++)
        {
            cardDisplays[i].SetCard(cardDatas[i]);
        }
        cardDisplays.ForEach((display) => display.UpdateVisibleDisplay());
    }

    /// <summary>
    /// 카드 선택 시작
    /// </summary>
    /// <param name="handDatas">손패 데이터</param>
    public void StartSelect(List<CardData> handDatas)
    {
        // 상태 변수 초기화
        selectedCards.Clear();
        selectEndFlag = false;

        Initialize(handDatas);

        StartCoroutine(CoRunSelect());
    }

    /// <summary>
    /// 메인 카드 선택 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoRunSelect()
    {
        // 카드 선택 시작
        cardDisplays.ForEach((display) => display.SetActiveInput(true)); // input 활성화
        Debug.Log("플레이어 카드 제출 기다리는 중");
        yield return new WaitUntil(() => selectEndFlag); // 제출 버튼 누를 때까지 기다리기

        // 플레이어 카드 제출 완료
        OnSelectEnd?.Invoke(selectedCards);
        selectedCards.ForEach((selectedCard) => DiscardCard(selectedCard)); // 카드 버리기

        cardDisplays.ForEach((display) => display.StateReset()); // 카드 UI 상태 초기화
        ResetState(); // 상태 변수 초기화
    }

    /// <summary>
    /// 카드 선택 이벤트 핸들러
    /// </summary>
    /// <param name="selectedCard"></param>
    private void HandleSelectCard(CardDisplay display)
    {
        CardData card = display.CardData;
        if (card == null) return;

        bool alreadySelected = selectedCards.Contains(card);

        if (!alreadySelected) // 카드 선택
        {
            if (!player.TrySpendEnergy(card.energyCost)) // currentEnergy와 비교, 판정
                return; // 에너지 부족 -> 무시, UI도 이미 흐려져 있어서 시각적으로 인지 가능

            // 선택된 카드 리스트에 추가
            selectedCards.Add(card);
            display.SetSelectedVisual(true);
        }
        else // 이미 클릭된 카드 선택 - 선택 카드 해제
        {
            player.RefundEnergy(card.energyCost);
            selectedCards.Remove(card);
            display.SetSelectedVisual(false);
        }
        OnCardSelect?.Invoke(player.CurrentEnergy);
    }

    /// <summary>
    /// 카드 제거
    /// </summary>
    /// <param name="card">제거할 카드</param>
    private void DiscardCard(CardData card)
    {
        FindCardDisplayByData(card).UpdateActiveCard(false);
        AffordableCards.Remove(card);
    }

    /// <summary>
    /// 데이터로 cardDisplay 찾기
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private CardDisplay FindCardDisplayByData(CardData card)
    {
        return cardDisplays.Find((cardDisplay) => (cardDisplay.CardId == card.cardId));
    }

    /// <summary>
    /// 상태 초기화
    /// </summary>
    private void ResetState()
    {
        selectedCards.Clear();
        selectEndFlag = false;
    }
    /// <summary>
    /// Discard되어 inactive된 Card Object들 모두 활성화
    /// 라운드 종료 이벤트 핸들러
    /// </summary>
    /// <param name="_"></param>
    private void FillCard()
    {
        // 카드 오브젝트 활성화
        cards.ForEach((card) => card.SetActive(true));
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResetAffordableCards()
    {
        affordableCards.Clear();
    }

    /// <summary>
    /// 라운드 마다 갱신되어야 하는 손패 정보 관리
    /// </summary>
    /// <param name="hand">손패</param>
    private void OnActorDrawCardHandler(List<CardData> hand)
    {
        affordableCards.AddRange(hand);
    }
}
