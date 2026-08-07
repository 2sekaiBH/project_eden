using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.Rendering.GPUSort;

/// <summary>
/// �÷��̾� �ൿ ���� ��ũ��Ʈ
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
    public static event Action OnPlayerStartSelect; // �÷��̾� ī�� ���� ���� �̺�Ʈ - npc slot btn, submit btn���� ����

    public void SetPlayer(string name, int maxHp)
    {
        this.name = name;
        this.maxHp = maxHp;
        Initialize();
    }

    public override void Initialize()
    {
        currentHp = maxHp;
        currentBlock = 0;
        currentEnergy = maxEnergy;
        
        profileUpdator.InitializeUpdator(maxHp, maxEnergy); // profileUpdator �ʱ�ȭ

        UpdateProfileUI();
    }

    // ī�� ���� ����
    public override void SelectCard()
    {
        OnPlayerStartSelect?.Invoke();
        handManager.StartSelect(hand);
    }


    public override void UpdateProfileUI()
    {
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, currentEnergy);
    }

    /// <summary>
    /// ������ ī�带 ��ü�ϴ� �Լ�
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cardData"></param>
    public CardData ReplaceCard(int index = 0, CardData cardData = null)
    {
        CardData newCard;
        if(cardData == null) // ī�带 �������� �ʾ����� ���� �̱�
        {
            newCard = DeckManager.Instance.DrawExtrCard(1)[0];
            hand[index] = newCard;
        }
        else
        {
            newCard = cardData;
            hand[index] = newCard; // ������ ī���?��ü
        }
        handManager.ReplaceHand(index, newCard); // ���� UI ���� 
        //UIUpdator.Instance.SetText($"�÷��̾� ���� {index + 1}��° ī�带 {newCard.name}���� �����߽��ϴ�.", CasterType.Player);
        Debug.Log($"�÷��̾� ���� {index + 1}��° ī�带 {newCard.name}���� �����߽��ϴ�.");
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

    // ī�� ����Ʈ ��ȯ ���� ������ refactoring �ʿ�
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
    /// ��Ÿ�� �� player�� maxEnergy �ʱ�ȭ ��� ���
    /// UI�� ǥ�õǴ� maxEnergy�� update
    /// </summary>
    /// <param name="maxEnergy">��ȭ�Ǵ� ��ġ</param>
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