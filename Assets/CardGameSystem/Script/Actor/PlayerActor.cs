using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½àµ¿ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Å©ï¿½ï¿½Æ®
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
    public static event Action OnPlayerStartSelect; // ÇÃ·¹ÀÌ¾î Ä«µå ¼±ÅÃ ½ÃÀÛ ÀÌº¥Æ® - npc slot btn, submit btn¿¡¼­ ±¸µ¶

    public override void Initialize()
    {
        maxHp = maxHpAmount;
        currentHp = maxHp;
        currentBlock = 0;
        currentEnergy = maxEnergy;
        
        profileUpdator.InitializeUpdator(maxHp, maxEnergy); // profileUpdator ÃÊ±âÈ­
    }

    // Ä«ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    public override void SelectCard()
    {
        OnPlayerStartSelect?.Invoke();
        handManager.StartSelect(hand);
    }

    void Awake()
    {
        name = "Player"; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ - Ä¿ï¿½ï¿½ï¿½ï¿½ nameï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        Initialize();
        UpdateProfileUI();
    }

    public override void UpdateProfileUI()
    {
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, currentEnergy);
    }

    /// <summary>
    /// ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Ä«ï¿½å¸¦ ï¿½ï¿½Ã¼ï¿½Ï´ï¿½ ï¿½Ô¼ï¿½
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cardData"></param>
    public void ReplaceCard(int index = 0, CardData cardData = null)
    {
        CardData newCard;
        if(cardData == null) // Ä«ï¿½å¸¦ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ê¾ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ì±ï¿½
        {
            newCard = DeckManager.Instance.DrawExtrCard(1)[0];
            hand[index] = newCard;
        }
        else
        {
            newCard = cardData;
            hand[index] = newCard; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Ä«ï¿½ï¿½ï¿?ï¿½ï¿½Ã¼
        }
        handManager.ReplaceHand(index, newCard); // ï¿½ï¿½ï¿½ï¿½ UI ï¿½ï¿½ï¿½ï¿½ 
        UIUpdator.Instance.SetText($"ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½ï¿½ï¿½ï¿½ {index + 1}ï¿½ï¿½Â° Ä«ï¿½å¸¦ {newCard.name}ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ß½ï¿½ï¿½Ï´ï¿½.");
        Debug.Log($"ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½ï¿½ï¿½ï¿½ {index + 1}ï¿½ï¿½Â° Ä«ï¿½å¸¦ {newCard.name}ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ß½ï¿½ï¿½Ï´ï¿½.");
    }

    public void DiscardCard(CardData card)
    {
        hand.Remove(card);
    }

    public override void EnergyIntialize()
    {
        SetEnergy(maxEnergy);
    }

    // Ä«ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Æ® ï¿½ï¿½È¯ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ refactoring ï¿½Ê¿ï¿½
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
    /// ·±Å¸ÀÓ Áß playerÀÇ maxEnergy ÃÊ±âÈ­ °æ¿ì »ç¿ë
    /// UI¿¡ Ç¥½ÃµÇ´Â maxEnergy¸¦ update
    /// </summary>
    /// <param name="maxEnergy">º¯È­µÇ´Â ¼öÄ¡</param>
    public void SetMaxEnergy(int amount)
    {
        this.maxEnergy += amount;
        profileUpdator.InitializeUpdator(maxHp, maxEnergy);
    }
}


