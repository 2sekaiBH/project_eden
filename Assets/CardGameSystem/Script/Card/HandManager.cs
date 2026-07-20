using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    /// <summary>
    /// 손패 관리 매니저
    /// Actor와 CardDisplay 사이의 인터페이스 역할
    /// </summary>

    [Header("Refernce")]
    [SerializeField] private List<GameObject> cards = new List<GameObject>(); // 카드 오브젝트들
    private List<CardDisplay> cardDisplays = new List<CardDisplay>(); // 카드 오브젝트에 부착된 CardDisplay

    private List<CardData> selectedCards = new List<CardData>(); // 선택된 카드 리스트
    private bool selectEndFlag = false; // 선택 종료 플래그
    public void HandleSelectEndFlag(bool value) // 제출 버튼에서 구독
    {
        selectEndFlag = value;
    }

    public event Action<List<CardData>> OnSelectEnd;
    // 플레이어 선택 최종 종료 이벤트
    // PlayerActor에서 구독

    private void Awake()
    {
        cards.ForEach((card) => cardDisplays.Add(card.GetComponent<CardDisplay>()));
    }

    // 이벤트 구독
    private void OnEnable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected += HandleSelectCard);
        RoundFlowManager.OnRoundEnd += FillCard;
    }
    // 이벤트 해제
    private void OnDisable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected -= HandleSelectCard);
    }

    /// <summary>
    /// 손패 업데이트
    /// </summary>
    public void Initialize(List<CardData> cardDatas)
    {
        for(int i = 0; i < cardDisplays.Count; i++)
        {
            cardDisplays[i].SetCard(cardDatas[i]);
        }
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
    private void HandleSelectCard(CardData selectedCard)
    {
        if (selectedCard == null) return;

        if (selectedCards.Contains(selectedCard)) // 선택 카드 삭제
        {
            selectedCards.Remove(selectedCard);
            
        }
        else if (!selectedCards.Contains(selectedCard)) // 선택 카드 추가
        {
            selectedCards.Add(selectedCard);
        }
    }

    /// <summary>
    /// 카드 제거
    /// </summary>
    /// <param name="card">제거할 카드</param>
    private void DiscardCard(CardData card)
    {
        FindCardDisplayByData(card).UpdateDiscardCard();
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
    private void FillCard(int _)
    {
        // 카드 오브젝트 활성화
        cards.ForEach((card) => card.SetActive(true));
    }
}
