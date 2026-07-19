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
    [SerializeField] private List<CardDisplay> cardDisplays;

    private List<CardData> selectedCards = new List<CardData>();
    private bool selectEndFlag = false;
    public void SelectEndFlag(bool value)
    {
        selectEndFlag = value;
    }

    public event Action<List<CardData>> OnSelectEnd;


    // 이벤트 구독
    private void OnEnable()
    {
        foreach (CardDisplay card in cardDisplays)
        {
            card.OnCardSelected += HandleSelectCard;

        }
    }
    // 이벤트 해제
    private void OnDisable()
    {
        foreach (CardDisplay card in cardDisplays)
        {
            card.OnCardSelected -= HandleSelectCard;
        }
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
        // 초기화
        selectedCards.Clear();
        selectEndFlag = false;
        Initialize(handDatas);

        StartCoroutine(CoRunSelect());
    } 

    private IEnumerator CoRunSelect()
    {
        foreach (var card in cardDisplays)
            card.SetActiveInput(true); // input 활성화
        Debug.Log("플레이어 카드 제출 기다리는 중");
        yield return new WaitUntil(() => selectEndFlag); // 제출 버튼 누를 때까지 기다리기

        // 제출 완료
        OnSelectEnd?.Invoke(selectedCards);
        foreach (CardData selectedCard in selectedCards) // 카드 버리기
        {
            DiscardCard(selectedCard);
        }

        foreach (var display in cardDisplays)
        {
            display.StateReset(); // 카드 UI 상태 초기화
        }

        ResetState(); // 상태 변수 초기화
    }

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

    private void DiscardCard(CardData card)
    {
        FindCardDisplayByData(card).DiscardCard();
    }

    private CardDisplay FindCardDisplayByData(CardData card)
    {
        return cardDisplays.Find((cardDisplay) => (cardDisplay.CardId == card.cardId));
    }

    private void ResetState()
    {
        selectedCards.Clear();
        selectEndFlag = false;
    }
}
