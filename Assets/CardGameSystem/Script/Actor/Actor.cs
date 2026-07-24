
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assemblies;

/// <summary>
/// 카드를 사용하는 주체(플레이어, 보스 등)
/// </summary>
public abstract class Actor: MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] protected ProfileUpdator profileUpdator;
    protected new string name;
    public int currentHp;
    protected int currentBlock = 0;
    protected int currentEnergy = 4;
    protected List<CardData> hand = new List<CardData>();
    public int CurrentHp => currentHp;
    public int CurrentBlock => currentBlock;
    public int CurrentEnergy => currentEnergy;

    public List<CardData> Hand => hand;
    private Action<int> _handler;

    private void OnEnable()
    {
        _handler = (amount) => UpdateProfileUI(); 
        TurnFlowManager.OnTurnStart += _handler; // turn 시작할 때 UI update
        TurnFlowManager.OnTurnEnd += ResetBlock;
    }

    private void OnDisEnable()
    {
        TurnFlowManager.OnTurnStart -= _handler;
        TurnFlowManager.OnTurnEnd -= ResetBlock;
    }

    /// <summary>
    /// Damage - Hp 감소 (기본 공격)
    /// </summary>
    /// <param name="amount">피해량</param>
    public virtual void TakeDamage(int amount)
    {
        int absorbed = Mathf.Min(currentBlock, amount);
        currentBlock -= absorbed;
        int remaining = amount - absorbed;
        currentHp = Mathf.Max(0, currentHp - remaining); // Hp 음수 방지

        Debug.Log($"{name}: {amount} 피해 중 {currentBlock} 막음. 현재 체력 {currentHp}");
        
        UpdateProfileUI();
    }

    /// <summary>
    /// Heal - Hp 증가
    /// </summary>
    /// <param name="amount">치료할 양</param>
    public virtual void Heal(int amount)
    {
        currentHp += amount;
        UpdateProfileUI();
    }

    /// <summary>
    /// 방어 증가
    /// </summary>
    /// <param name="amount">증가할 양</param>
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
    /// 에너지 소비
    /// </summary>
    /// <param name="amount">소비량</param>
    public virtual bool TrySpendEnergy(int amount)
    {
        if (currentEnergy < amount) return false;
        SetEnergy(currentEnergy - amount);
        return true;
    }

    /// <summary>
    /// 에너지 추가
    /// </summary>
    /// <param name="amount">추가할 양</param>
    public virtual void RefundEnergy(int amount)
    {
        SetEnergy(currentEnergy + amount);
    }

    /// <summary>
    /// 에너지 설정
    /// </summary>
    /// <param name="value"> 설정할 에너지 값</param>
    public void SetEnergy(int value)
    {
        currentEnergy = Mathf.Max(0, value);
        UpdateProfileUI(); // 에너지 변경 = 프로필 UI 갱신, 항상 같이 일어남을 여기서 보장
    }

    /// <summary>
    /// 에너지 초기화 - 매 턴 시작마다 실행
    /// </summary>
    /// 
    public abstract void EnergyIntialize(); 

    /// <summary>
    /// 덱에서 카드 뽑기
    /// </summary>
    /// <param name="amount">가져올 카드 수</param>
    public virtual void DrawCards(int amount)
    {
        hand.AddRange(DeckManager.Instance.DrawRandomCard(amount));
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
}
