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
        roundPendingEffect.extraCards.Clear(); // 재사용 방지 - 한번 적용 후 clear;
    }

    void Update()
    {

    }

    // 평타 공격 강화
    public void AddExtraAttack(int damage)
    {
        turnPendingEffect.extraAttack.Add(new PendingExtraAttackEffect(damage));
        
    }

    // 턴 끝마다 공격 설정
    public void AddEndturnDamage(int damge, int turn, Actor target)
    {
        turnPendingEffect.endTurnDamageEffect.Add(new PendingEndTurnDamageEffect(damge, turn, target));
    }

    // 평타 추가 공격 실행
    public int ConsumeExtraAttack()
    {
        int totalDamage = 0;

        foreach(PendingExtraAttackEffect extra in turnPendingEffect.extraAttack)
        {
            totalDamage += extra.damage;
        } //리스트에 있는 목록을 차례대로 실행

        turnPendingEffect.extraAttack.Clear(); // 재사용 방지 - 한번 적용 후 clear;

        return totalDamage;
      
    }

    // 끝날 때 추가 공격 실행
    public void ConsumeEndturnDamage()
    {
       foreach (PendingEndTurnDamageEffect endturnDamage in turnPendingEffect.endTurnDamageEffect)
        {
            if (endturnDamage.remainTurn <= 0)
                continue;

            endturnDamage.target.TakeDamage(endturnDamage.damage, null);
            endturnDamage.remainTurn--;

            Debug.Log($"턴 종료 {endturnDamage.remainTurn} 남음");
        }

        // 턴이 끝난 효과 제거
        turnPendingEffect.endTurnDamageEffect.RemoveAll(effect => effect.remainTurn <= 0);
    }

    // 에너지 코스트 -1 설정할 Actor를 들고 옴
    public void ReduceCost(Actor player)
    {
        turnPendingEffect.reduceCost.Add(new PendingReduceCostEffect(player));
    }

    // 에너지 코스트 -1 사용
    public void ConsumeReduceCost()
    {
       foreach(PendingReduceCostEffect reduceCost in turnPendingEffect.reduceCost)
        {
            if (reduceCost.target == null)
                continue;

            reduceCost.target.EnableReduceCost();
        }

        turnPendingEffect.reduceCost.Clear(); // 재사용 방지 - 한번 적용 후 clear;
    }

    //어떤 카드를 추가로 받을지, 누가 받을지 설정
    public void AddExtraCard(Actor getCard, CardData card)
    {
        turnPendingEffect.extraCards.Add(new PendingCardEffect(getCard, card));
    }

    //카드 추가 지급 효과 적용
    public void ConsumeExtraCard()
    {
     if(turnPendingEffect.extraCards.Count <= 0)
            return;

       foreach(PendingCardEffect effect in turnPendingEffect.extraCards)
        {
            effect.getCard.AddCard(effect.card);
        }

     turnPendingEffect.extraCards.Clear(); // 재사용 방지 - 한번 적용 후 clear;
     
    }
}

[System.Serializable]
public class TurnPendingEffect
{

    //평타 추가 공격 기억
    public List<PendingExtraAttackEffect> extraAttack = new List<PendingExtraAttackEffect>();

    // 2턴간 -3공격 기억
    public List<PendingEndTurnDamageEffect> endTurnDamageEffect = new List<PendingEndTurnDamageEffect>();

    // 카드 코스트 -1 기억
    public List<PendingReduceCostEffect> reduceCost = new List<PendingReduceCostEffect>();

    //추가 카드를 받을 대상, 카드 설정
    public List<PendingCardEffect> extraCards = new List<PendingCardEffect>();
    
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

//카드 추가 지급을 리스트로 관리하기 위한 클래스
[System.Serializable]
public class PendingCardEffect
{

   public Actor getCard;
    public CardData card;

    public PendingCardEffect(Actor getCard, CardData card)
    {
        this.getCard = getCard;
        this.card = card;
    }
}

//평타 추가 공격을 리스트로 관리하기 위한 클래스
[System.Serializable]
public class PendingExtraAttackEffect
{
    public int damage;

    public PendingExtraAttackEffect(int damage)
    {
        this.damage = damage;
    }
}

//턴 종료시 공격을 리스트로 관리하기 위한 클래스
[System.Serializable]
public class PendingEndTurnDamageEffect
{
    public int damage;
    public int remainTurn;
    public Actor target;

    public PendingEndTurnDamageEffect(
        int damage,
        int remainTurn,
        Actor target)
    {
        this.damage = damage;
        this.remainTurn = remainTurn;
        this.target = target;
    }
}

//에너지 코스트 -1을 리스트로 관리하기 위한 클래스
[System.Serializable]
public class PendingReduceCostEffect
{
    public Actor target;

    public PendingReduceCostEffect(Actor target)
    {
        this.target = target;
    }
}