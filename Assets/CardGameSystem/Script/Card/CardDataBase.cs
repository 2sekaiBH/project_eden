using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataBase", menuName = "Scriptable Objects/CardSystem/CardDataBase")]
public class CardDataBase : ScriptableObject
{
    [SerializeField] private List<CardData> _cardDataBase;
    public List<CardData> cardDataBase => _cardDataBase;
}
