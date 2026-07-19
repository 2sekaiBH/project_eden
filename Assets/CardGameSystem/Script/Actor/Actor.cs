
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
    protected List<CardData> hand = new List<CardData>();
    public int CurrentHp => currentHp;
    public int CurrentBlock => currentBlock;
    public List<CardData> Hand => hand;

    public virtual void TakeDamage(int amount)
    {
        int absorbed = Mathf.Min(currentBlock, amount);
        currentBlock -= absorbed;
        int remaining = amount - absorbed;
        currentHp = Mathf.Max(0, currentHp - remaining); // Hp 음수 방지
        currentHp -= amount;
    }

    public virtual void Heal(int amount)
    {
        currentHp += amount;
    }

    public virtual void AddBlock(int amount)
    {
        currentBlock += amount;
    }

    public virtual void DrawCards(int amount)
    {
        hand.AddRange(DeckManager.Instance.DrawRandomCard(amount));
    }

    public abstract void SelectCard();

    public abstract void Initialize();
}
