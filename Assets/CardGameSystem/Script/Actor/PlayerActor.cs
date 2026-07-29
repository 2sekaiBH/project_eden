using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    [SerializeField] private int maxHp;

    public event Action<List<CardData>> OnPlayerDrawCard;

    public override void Initialize()
    {
        currentHp = maxHp;
        currentBlock = 0;
        currentEnergy = maxEnergy;
    }

    // ī�� ���� ����
    public override void SelectCard()
    {
        handManager.StartSelect(hand);
    }

    void Awake()
    {
        name = "Player"; // ������ - Ŀ���� name���� ����
        Initialize();
        UpdateProfileUI();
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
    public void ReplaceCard(int index = 0, CardData cardData = null)
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
            hand[index] = newCard; // ������ ī��� ��ü
        }
        handManager.ReplaceHand(index, newCard); // ���� UI ���� 
        UIUpdator.Instance.SetText($"�÷��̾� ���� {index + 1}��° ī�带 {newCard.name}���� �����߽��ϴ�.");
        Debug.Log($"�÷��̾� ���� {index + 1}��° ī�带 {newCard.name}���� �����߽��ϴ�.");
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
}


