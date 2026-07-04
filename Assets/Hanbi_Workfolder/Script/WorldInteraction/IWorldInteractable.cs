using System.Globalization;
using System.Xml.Serialization;
using UnityEngine;

public interface IWorldInteractable
{
    string InteractionId { get; }
    InteractionType InteractionType { get; }

    /// <summary>
    /// 상호작용이 가능한지 - 충족 조건
    /// </summary>
    /// <param name="player"></param>
    bool CanInteract(PlayerController player);
    /// <summary>
    /// 구체적인 상호작용 구현
    /// </summary>
    void Interact(PlayerController player);
}
