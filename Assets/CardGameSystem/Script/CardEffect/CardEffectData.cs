using UnityEngine;
using CardSystem.Runtime;
using CardSystem.Effects;

/// <summary>
/// 카드 한 장이 실제로 어떤 일을 하는지를 나타내는 최소 단위.
/// CardData.effects 리스트에 이 클래스를 여러 개 조합해서 카드 동작을 만든다.
/// ScriptableObject라서 인스펙터에서 카드마다 재사용/조합이 가능하다.
/// </summary>
public abstract class CardEffectData : ScriptableObject
{
    public abstract void Execute(CardContext context);
}

