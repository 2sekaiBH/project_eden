using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 저번 턴에서의 변동사항을 저장했다가 턴 시작때 적용하는 스크립트
/// </summary>
public class PendingEffectManager : MonoBehaviour
{
    private static PendingEffectManager instance;
    public static PendingEffectManager Instance => instance;

    private RoundPendingEffect roundPendingEffect;
    private TurnPendingEffect turnPendingEffect;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        roundPendingEffect = new RoundPendingEffect();
        turnPendingEffect = new TurnPendingEffect();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetRoundPendingEffect(CardData cardData = null)
    {
        Debug.Log($"roundPendingState에 {cardData.name} 추가됨.");
        roundPendingEffect.AddExtraCard(cardData);
    }


    public void ApplyRoundPendingState(PlayerActor playerActor, OpponentActor opponentActor)
    {
        if(roundPendingEffect.extraCards.Count > 0)
        {
            roundPendingEffect.extraCards.ForEach((card) => playerActor.AddCard(card));
            Debug.Log($"라운드 시작 - 플레이어에게 {roundPendingEffect.extraCards.Select((card) => card.name)} 카드 전달, 플레이어 카드 수: {playerActor.Hand.Count}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}


[System.Serializable]
public class TurnPendingEffect
{
    
}

[System.Serializable]
public class RoundPendingEffect
{
    public List<CardData> extraCards = new List<CardData>();

    public RoundPendingEffect()
    {
        
    }

    public List<CardData> AddExtraCard(CardData extraCard)
    {
        extraCards.Add(extraCard);
        return extraCards;
    }
}