using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardSystem/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public int energyCost;
    [TextArea] public string description;

    public List<CardEffectData> effects;
}

public enum CardType
{
    Attack,
    Defense,
    Special,
}