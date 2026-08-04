using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 플레이어 행동 제어 스크립트
/// </summary>
public class PlayerActor : Actor
{
    [Header("Reference")]
    [SerializeField] private HandManager handManager;
    public HandManager HandManager => handManager;

    [Header("Setting")]
    [SerializeField] private int maxEnergy;
    [SerializeField] private int maxHpAmount;

    public event Action<List<CardData>> OnPlayerDrawCard;
    public static event Action OnPlayerStartSelect; // 플레이어 카드 선택 시작 이벤트 - npc slot btn, submit btn에서 구독

    public override void Initialize()
    {
        name = "Player"; // 디버깅용 - 커스텀 name으로 변경
        maxHp = maxHpAmount;
        currentHp = maxHp;
        currentBlock = 0;
        currentEnergy = maxEnergy;
        
        profileUpdator.InitializeUpdator(maxHp, maxEnergy); // profileUpdator 초기화
    }

    // 카드 선택 시작
    public override void SelectCard()
    {
        OnPlayerStartSelect?.Invoke();
        handManager.StartSelect(hand);
    }

    void Awake()
    {
        Initialize();
        UpdateProfileUI();
    }

    public override void UpdateProfileUI()
    {
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, currentEnergy);
    }

    /// <summary>
    /// 손패의 카드를 교체하는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cardData"></param>
    public CardData ReplaceCard(int index = 0, CardData cardData = null)
    {
        CardData newCard;
        if(cardData == null) // 카드를 지정하지 않았으면 랜덤 뽑기
        {
            newCard = DeckManager.Instance.DrawExtrCard(1)[0];
            hand[index] = newCard;
        }
        else
        {
            newCard = cardData;
            hand[index] = newCard; // 지정한 카드로 교체
        }
        handManager.ReplaceHand(index, newCard); // 손패 UI 갱신 
        //UIUpdator.Instance.SetText($"플레이어 손패 {index + 1}번째 카드를 {newCard.name}으로 변경했습니다.", CasterType.Player);
        Debug.Log($"플레이어 손패 {index + 1}번째 카드를 {newCard.name}으로 변경했습니다.");
        return newCard;
    }

    public void DiscardCard(CardData card)
    {
        hand.Remove(card);
    }

    public override void EnergyIntialize()
    {
        SetEnergy(maxEnergy);
    }

    // 카드 리스트 변환 없는 구조로 refactoring 필요
    public override void SetHand(CardData card)
    {
        base.SetHand(card);
        List<CardData> cardList = new List<CardData>();
        cardList.Add(card);
        OnPlayerDrawCard?.Invoke(cardList);
    }

    public override void SetHand(List<CardData> cards)
    {
        base.SetHand(cards);
        OnPlayerDrawCard?.Invoke(cards);
    }

    /// <summary>
    /// 런타임 중 player의 maxEnergy 초기화 경우 사용
    /// UI에 표시되는 maxEnergy를 update
    /// </summary>
    /// <param name="maxEnergy">변화되는 수치</param>
    public void SetMaxEnergy(int amount)
    {
        this.maxEnergy += amount;
        profileUpdator.InitializeUpdator(maxHp, maxEnergy);
    }
}

public enum Caster
{
    Player,
    Opponent,
    System
}