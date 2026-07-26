using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 행동 제어 스크립트
/// </summary>
public class OpponentActor : Actor
{
    [SerializeField] private OpponentData opponentData; // name, Hp

    public event Action<List<CardData>> OnOpponentEndSelect;
    void Awake()
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
    /// 가진 에너지를 최대한 사용하여 카드 제출
    /// </summary>
    public override void SelectCard()
    {
        List<CardData> pickedCard = new List<CardData>();
        int i = 0;
        while (TrySpendEnergy(hand[i].energyCost))
        {
            pickedCard.Add(hand[i]);
            i++;
        }
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
