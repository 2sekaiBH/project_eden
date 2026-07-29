using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager Instance;

    [SerializeField] private int currentGauge = 0;
    [SerializeField] public int maxGauge = 12;
    [SerializeField] public Sprite gaugeImage;
    [SerializeField] public Sprite maxGaugeImage;

    public int CurrentGauge => currentGauge;
    public int MaxGauge => maxGauge;

    public event Action<int, int> OnGaugeChange; //집중 게이지가 변하면 바꼈다고 알려줄 이벤트
    public event Action OnBurst; //게이지가 최대치에 도달했는지 알려줄 이벤트


    //첫 번째로 생성된 GaugeManager만 남기고 나머지 삭제

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
    }

    //게이지 초기화
    public void ResetGauge()
    {
        currentGauge = 0;
        OnGaugeChange?.Invoke(currentGauge, maxGauge);
    }

    //게이지 증가 함수
    public void AddGauge(int amount)
    {
        currentGauge = Mathf.Min(currentGauge + amount, maxGauge); //게이지가 최대치를 넘지 않도록 함
        OnGaugeChange?.Invoke(currentGauge, maxGauge); //게이지 변화를 알려줌
        Debug.Log($"현재 게이지: {currentGauge}");

    }

    //게이지 최대 도달 시 공격을 수행하는 함수! 이후 게이지는 초기화
    public void Burst()
    {
        OnBurst?.Invoke();
        ResetGauge();
    }

    //플레이어가 이번 턴에 에너지를 다 썼는지 확인
    public void AllEnergy(PlayerActor player)
    {
        if(player.CurrentEnergy == 0)
        {
            AddGauge(2);
            Debug.Log("에너지 전부 사용, 게이지 +2");
        }
    }

    //같은 타입의 카드만 제출했는지 확인
    public void SameCardType(List<CardData> cards)
    {
        if (cards == null || cards.Count == 0)
            return;

        CardType first = cards[0].cardType; //첫 번째 카드의 타입을 가져옴!

        //카드 타입이 첫 번째와 다르면 리턴
        foreach (CardData card in cards)
        {
            if (card.cardType != first)
                return;
        }

        AddGauge(2);
        Debug.Log("같은 종류의 카드만 사용, +2");
        
    }

    //조건부 카드 사용 확인
    public void UseAdaptive(List<CardData> cards)
    {
        foreach(CardData card in cards)
        {
            if(card.isMissionCard)
            {
                AddGauge(3);
                Debug.Log("조건부 카드 사용 확인 +3");
            }
        }
    }
}
