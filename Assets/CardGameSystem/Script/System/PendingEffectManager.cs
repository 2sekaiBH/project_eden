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

    void Start()
    {

    }

    public void SetRoundPendingEffect(List<CardData> cardDatas = null)
    {
        Debug.Log($"roundPendingState에 {cardDatas.Select(card => card.name)} 추가됨.");
        roundPendingEffect.AddExtraCard(cardDatas);
    }

    public void ApplyRoundPendingState(PlayerActor playerActor, OpponentActor opponentActor)
    {
        if (roundPendingEffect.extraCards.Count > 0)
        {
            roundPendingEffect.extraCards.ForEach((card) => playerActor.AddCard(card));
            Debug.Log($"라운드 시작 - 플레이어에게 {roundPendingEffect.extraCards.Select((card) => card.name)} 카드 전달, 플레이어 카드 수: {playerActor.Hand.Count}");
        }
    }

    void Update()
    {

    }

    // 평타 공격 강화
    public void AddExtraAttack(int damage)
    {
        turnPendingEffect.extraDefaultDamage = damage;
    }

    // 턴 끝마다 공격 설정
    public void AddEndturnDamage(int damge, int turn, Actor target)
    {
        turnPendingEffect.endTurnDamage = damge;
        turnPendingEffect.endTurnDamageRemain = turn;
        turnPendingEffect.target = target;
    }

    // 평타 추가 공격 실행
    public int ConsumeExtraAttack()
    {
        int damage = turnPendingEffect.extraDefaultDamage;
        turnPendingEffect.extraDefaultDamage = 0;
        return damage;
    }

    // 끝날 때 추가 공격 실행
    public void ConsumeEndturnDamage()
    {
        if (turnPendingEffect.endTurnDamageRemain <= 0)
            return;

        turnPendingEffect.target.TakeDamage(turnPendingEffect.endTurnDamage, null);
        turnPendingEffect.endTurnDamageRemain--;
    }

    // 에너지 코스트 -1 설정할 Actor를 들고 옴
    public void ReduceCost(Actor player)
    {
        turnPendingEffect.player = player;
    }

    // 에너지 코스트 -1 사용
    public void ConsumeReduceCost()
    {
        if (turnPendingEffect.player == null)
            return;

        turnPendingEffect.player.EnableReduceCost();
        turnPendingEffect.player = null;
    }
}

[System.Serializable]
public class TurnPendingEffect
{
    public int extraDefaultDamage = 0; // 평타 추가 공격 기억

    // 2턴간 -3공격 기억
    public int endTurnDamage;
    public int endTurnDamageRemain = 0;
    public Actor target;

    // 카드 코스트 -1 대상
    public Actor player;
}

[System.Serializable]
public class RoundPendingEffect
{
    public List<CardData> extraCards = new List<CardData>();

    public RoundPendingEffect()
    {

    }

    public List<CardData> AddExtraCard(List<CardData> extraCard)
    {
        extraCards.AddRange(extraCard);
        return extraCards;
    }
}