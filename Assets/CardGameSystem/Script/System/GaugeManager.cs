using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager Instance;

    [SerializeField] private int currentGauge = 0;
    [SerializeField] private int maxGauge = 12;
    [SerializeField] private Image gaugeImage;
    [SerializeField] private Image maxGaugeImage;
    [SerializeField] private TMP_Text gaugePercentText;

    public int CurrentGauge => currentGauge;
    public int MaxGauge => maxGauge;

    public event Action OnBurst; //게이지가 최대치에 도달했는지 알려줄 이벤트


    //첫 번째로 생성된 GaugeManager만 남기고 나머지 삭제

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        UpdateGaugeUI();
    }

    //게이지 초기화
    public void ResetGauge()
    {
        currentGauge = 0;
        UpdateGaugeUI();
    }

    //게이지 증가 함수
    public void AddGauge(int amount)
    {
        currentGauge = Mathf.Min(currentGauge + amount, maxGauge); //게이지가 최대치를 넘지 않도록 함
        UpdateGaugeUI();
        Debug.Log($"현재 게이지: {currentGauge}");

    }

//게이지 UI변경
    private void UpdateGaugeUI()
    {
        float percent = (float)currentGauge / maxGauge;

        targetFill = percent;
        gaugePercentText.text = $": {Mathf.RoundToInt(percent * 100)}%";

    }

    private float targetFill = 0f;
    [SerializeField] private float gaugeSpeed = 1f; //게이지 차는 속도 조절

    private void Update() //게이지가 차오르고 내려가는 거 업데이트, 유니티가 자동으로 업데이트 해줌
    {
        maxGaugeImage.fillAmount = Mathf.MoveTowards(
            maxGaugeImage.fillAmount,
            targetFill,
            gaugeSpeed * Time.deltaTime);
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
