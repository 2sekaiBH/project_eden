using System;
using System.Collections.Generic;
using UnityEngine;

public class OpponentActor : Actor
{
    [SerializeField] private OpponentData opponentData; // name, Hp

    public event Action<List<CardData>> OnOpponentEndSelect;
    void Start()
    {
        name = opponentData.name;
        Initialize();
        UpdateProfileUI();
    }
    public override void Initialize()
    {
        currentHp = opponentData.totalHp;
        currentBlock = 0;
        currentEnergy = opponentData.maxEnergy;
    }

    /// <summary>
    /// 적의 카드 select 로직
    /// 손패 중 랜덤으로 하나 뽑기
    /// </summary>
    public override void SelectCard()
    {
        List<CardData> pickedCard = new List<CardData>();
        pickedCard.Add(hand[UnityEngine.Random.Range(0, hand.Count)]);
        OnOpponentEndSelect.Invoke(pickedCard);
    }

    public override void UpdateProfileUI()
    {
        profileUpdator.UpdateProfile(name, currentHp, currentBlock);
    }

    public override void EnergyIntialize()
    {
        SetEnergy(opponentData.maxEnergy);
    }
}
