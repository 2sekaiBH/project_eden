using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.Rendering;

/// <summary>
/// 카드를 사용하는 주체(플레이어, 보스 등)
/// </summary>
public abstract class Actor : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] protected ProfileUpdator profileUpdator;

    protected new string name;
    protected int currentHp;
    protected int currentBlock = 0;
    protected int currentEnergy = 4;

    public List<CardData> hand = new List<CardData>();


    public int CurrentHp => currentHp;
    public int CurrentBlock => currentBlock;
    public int CurrentEnergy => currentEnergy;
    public List<CardData> Hand => hand;

    private Action<int> _handler;

    /// <summary>
    /// 상대방 데미지 반사 상태
    /// </summary>
    private bool reflect = false;

    // 데미지 절반으로 받는 상태
    private bool halfDamage = false;

    // 에너지가 -1인 상태
    private bool reduceCost = false;

    private void OnEnable()
    {
        _handler = (amount) => UpdateProfileUI();

        TurnFlowManager.OnTurnStart += _handler;
        TurnFlowManager.OnTurnEnd += ResetBlock;
        RoundFlowManager.OnRoundStart += ResetHand;
    }

    private void OnDisEnable()
    {
        TurnFlowManager.OnTurnStart -= _handler;
        TurnFlowManager.OnTurnEnd -= ResetBlock;
        RoundFlowManager.OnRoundStart -= ResetHand;
    }

    /// <summary>
    /// Damage - Hp 감소 (기본 공격)
    /// </summary>
    public virtual void TakeDamage(int amount, Actor attacker)
    {
        int origin = amount;

        // 데미지 절반 구현
        if (halfDamage)
        {
            amount = Mathf.CeilToInt(amount * 0.5f);
            Debug.Log($"데미지 절반 적용! {origin} -> {amount}");
        }

       
        int absorbed = Mathf.Min(currentBlock, amount);
        currentBlock -= absorbed;

        Debug.Log($"{name}: {amount} 피해 중 {absorbed} 막음");


        int remaining = amount - absorbed;
        currentHp = Mathf.Max(0, currentHp - remaining);
        MissionManager.Instance.TakeDamage(this); //데미지를 입었는지 확인

        // 반사 구현
        if (reflect && absorbed > 0 && attacker != null)
        {
            attacker.TakeDamage(absorbed, null);
        }

        Debug.Log($"{name}: 현재 체력: {currentHp}");
        UpdateProfileUI();
    }

    /// <summary>
    /// Heal - Hp 증가
    /// </summary>
    public virtual void Heal(int amount)
    {
        currentHp += amount;
        UpdateProfileUI();
    }

    /// <summary>
    /// 방어 증가
    /// </summary>
    public virtual void AddBlock(int amount)
    {
        currentBlock += amount;
        UpdateProfileUI();
    }

    /// <summary>
    /// 방어 리셋 - 매 턴 종료마다 실행
    /// </summary>
    public virtual void ResetBlock(int _)
    {
        currentBlock = 0;
        Debug.Log("방어 리셋");
    }

    /// <summary>
    /// 손패 리셋 - 매 라운드 시작마다 실행
    /// </summary>
    public virtual void ResetHand(int _)
    {
        hand.Clear();
    }

    /// <summary>
    /// 에너지 소비
    /// </summary>
    public virtual bool TrySpendEnergy(int amount)
    {
        int origin = amount;

        // 실제 카드 코스트를 1 줄임
        if (reduceCost)
        {
            amount = Mathf.Max(0, amount - 1);
            Debug.Log($"카드 코스트 요래됐수 {origin} -> {amount}");
        }

        if (currentEnergy < amount)
            return false;

        SetEnergy(currentEnergy - amount);
        return true;
    }

    /// <summary>
    /// 에너지 추가
    /// </summary>
    public virtual void RefundEnergy(int amount)
    {
        SetEnergy(currentEnergy + amount);
    }

    /// <summary>
    /// 에너지 설정
    /// </summary>
    public void SetEnergy(int value)
    {
        currentEnergy = Mathf.Max(0, value);
        UpdateProfileUI();
    }

    /// <summary>
    /// 에너지 초기화 - 매 턴 시작마다 실행
    /// </summary>
    public abstract void EnergyIntialize();

    /// <summary>
    /// 덱에서 카드 뽑기
    /// </summary>
    public virtual void DrawCards(int amount)
    {
        SetHand(DeckManager.Instance.DrawRandomCard(amount));
    }

    /// <summary>
    /// 카드 선택
    /// </summary>
    public abstract void SelectCard();

    /// <summary>
    /// 초기화
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// 프로필 UI 업데이터
    /// </summary>
    public abstract void UpdateProfileUI();

    // 반사 상태 On
    public void EnableReflect()
    {
        reflect = true;
    }

    // 데미지 절반 적용
    public void EnableHalfDamage()
    {
        halfDamage = true;
    }

    // 카드 코스트 -1
    public void EnableReduceCost()
    {
        reduceCost = true;
    }

    // 턴 동안 지속되는 효과 초기화
    public void ResetTurnEffect()
    {
        reflect = false;
        halfDamage = false;
        reduceCost = false;
    }

    /// <summary>
    /// 여러 카드를 인자로 받는 hand Setter
    /// </summary>
    /// <param name="cards"></param>
    public virtual void SetHand(List<CardData> cards)
    {
        hand.AddRange(cards);
    }

    /// <summary>
    /// 카드 한개용 hand Setter
    /// </summary>
    /// <param name="card"></param>
    public virtual void SetHand(CardData card)
    {
        hand.Add(card);
    }


    /// <summary>
    /// 기본 카드 드로우 외 특정한 카드를 추가하는 메소드
    /// </summary>
    /// <param name="cardData">추가할 카드</param>
    public virtual void AddCard(CardData cardData)
    {
        if (cardData != null)
            SetHand(cardData);
    }

}