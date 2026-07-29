using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 행동 제어 스크립트
/// </summary>
public class OpponentActor : Actor
{
    [SerializeField] private OpponentData opponentData; // name, Hp

    public static event Action<List<CardData>> OnOpponentEndSelect;

    private bool isEnabledSelect = true; // 아키텍트의 - 적 턴 스킵 효과 구현을 위해 사용

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
        if (this.isEnabledSelect)
        {
            int i = 0;
            while (i < hand.Count && TrySpendEnergy(hand[i].energyCost))
            {
                pickedCard.Add(hand[i]);
                i++;
            }
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

    /// <summary>
    /// 아키텍트 효과 구현 - 이번 턴 적 select 스킵
    /// turn 종료 시 해제
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveOnCurrentTurn(bool active)
    {
        isEnabledSelect = active;
        profileUpdator.UpdateActiveProfile(active);
    }
}
