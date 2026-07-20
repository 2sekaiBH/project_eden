
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assemblies;

/// <summary>
/// 카드를 사용하는 주체(플레이어, 보스 등)가 구현해야 하는 최소 인터페이스.
/// CardEffect들은 구체 클래스가 아니라 이 인터페이스에만 의존한다.
/// </summary>
public abstract class Actor: MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] protected ProfileUpdator profileUpdator;
    protected new string name;
    protected int currentHp;
    protected int currentBlock = 0;
    protected int currentEnergy = 4;
    protected List<CardData> hand = new List<CardData>();
    public int CurrentHp => currentHp;
    public int CurrentBlock => currentBlock;
    public int CurrentEnergy => currentEnergy;
    public List<CardData> Hand => hand;

    /// <summary>
    /// Damage - Hp 감소
    /// </summary>
    /// <param name="amount">피해량</param>
    public virtual void TakeDamage(int amount)
    {
        int absorbed = Mathf.Min(currentBlock, amount);
        currentBlock -= absorbed;
        int remaining = amount - absorbed;
        currentHp = Mathf.Max(0, currentHp - remaining); // Hp 음수 방지
        currentHp -= amount;
    }

    /// <summary>
    /// Heal - Hp 증가
    /// </summary>
    /// <param name="amount">치료할 양</param>
    public virtual void Heal(int amount)
    {
        currentHp += amount;
    }

    /// <summary>
    /// 방어 증가
    /// </summary>
    /// <param name="amount">증가할 양</param>
    public virtual void AddBlock(int amount)
    {
        currentBlock += amount;
    }

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
}
