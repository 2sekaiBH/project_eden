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

        roundPendingEffect = new RoundPendingEffect(null);
        turnPendingEffect = new TurnPendingEffect();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetRoundPendingEffect(CardData cardData = null)
    {
        Debug.Log($"roundPendingState에 {cardData.name} 추가됨.");
        roundPendingEffect.extraCard = cardData;
    }


    public void ApplyRoundPendingState(PlayerActor playerActor, OpponentActor opponentActor)
    {
        playerActor.AddCard(roundPendingEffect.extraCard);
        Debug.Log($"라운드 시작 - 플레이어에게 {roundPendingEffect.extraCard} 카드 전달, 플레이어 카드 수: {playerActor.Hand.Count}");
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
    public CardData extraCard;

    public RoundPendingEffect(CardData extraCard)
    {
        this.extraCard = extraCard;
    }
}