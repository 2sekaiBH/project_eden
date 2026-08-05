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
    /// 카드 실행 로직 - 전달받은 cardData의 effect들을 실행
    /// </summary>
    /// <param name="cardList">실행할 카드들</param>
    /// <param name="caster">시전자</param>
    /// <param name="target">대상</param>
    public void CardExecute(List<CardData> cardList, Actor caster, Actor target)
    {
        string cards = "";
        cardList.ForEach(card => cards += card.description);
        Debug.Log($"{caster}의 카드 {cards}");

        CardContext context = new CardContext(caster, target);
        foreach (CardData card in cardList)
        {
            card.effects.ForEach((effect) => effect.Execute(context)); // 카드 effect 실행
            caster.TrySpendEnergy(card.energyCost); // energy 소비
        }
    }
}
