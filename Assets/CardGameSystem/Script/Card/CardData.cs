using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardSystem/CardData")]
public class CardData : ScriptableObject
{
    public int cardId;
    public string cardName;
    public CardType cardType;
    public int energyCost;
    public string effect;
    [TextArea] public string description;

    public List<CardEffectData> effects;
}

public enum CardType
{
    Attack = 0,
    Defense,
    Special,
}