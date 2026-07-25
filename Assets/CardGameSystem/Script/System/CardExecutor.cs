using UnityEngine;
using CardSystem.Runtime;
using System.Collections.Generic;
using System;

/// <summary>
/// 전달받은 카드의 실행을 제어하는 스크립트
/// </summary>
public class CardExecutor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// 카드 실행 제어
    /// 플레이어, 적 카드 동시 처리
    /// </summary>
    /// <param name="playerActor"></param>
    /// <param name="playerSelectedCard"></param>
    /// <param name="opponentActor"></param>
    /// <param name="opponentSelectedCards"></param>
    public void CardExecuteControll(Actor playerActor, List<CardData> playerSelectedCard, Actor opponentActor, List<CardData> opponentSelectedCards)
    {
        // 방어 카드 먼저 실행
        CardExecute(playerSelectedCard.FindAll(card => card.cardType == CardType.Defense), playerActor, opponentActor);
        CardExecute(opponentSelectedCards.FindAll(card => card.cardType == CardType.Defense), opponentActor, playerActor);

        // 공격 카드 실행
        CardExecute(playerSelectedCard.FindAll(card => card.cardType == CardType.Attack), playerActor, opponentActor);
        CardExecute(opponentSelectedCards.FindAll(card => card.cardType == CardType.Attack), opponentActor, playerActor);

        // 특수 카드 실행
        CardExecute(playerSelectedCard.FindAll(card => card.cardType == CardType.Special), playerActor, opponentActor);
        CardExecute(opponentSelectedCards.FindAll(card => card.cardType == CardType.Special), opponentActor, playerActor);
    }

    /// <summary>
    /// 카드 실행 로직 - 전달받은 cardData의 effect들을 실행
    /// </summary>
    /// <param name="cardList">실행할 카드들</param>
    /// <param name="caster">시전자</param>
    /// <param name="target">대상</param>
    private void CardExecute(List<CardData> cardList, Actor caster, Actor target)
    {
        string cards = "";
        cardList.ForEach(card => cards += card.description);
        Debug.Log($"{caster}의 카드 {cards}");

        CardContext context = new CardContext(caster, target);
        foreach (CardData card in cardList)
        {
            card.effects.ForEach((effect) => effect.Execute(context)); // 카드 effect 실행
        }
    }
}
