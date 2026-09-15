using System.Globalization;
using System.Xml.Serialization;
using UnityEngine;

public interface IWorldInteractable
{
    /// <summary>
    /// 상호작용 ID - 상호작용을 구분하기 위한 고유 ID
    /// </summary>
    int InteractionId { get; }
    /// <summary>
    /// 상호작용 타입
    /// </summary>
    InteractionType InteractionType { get; }
    /// <summary>
    /// 상호작용이 가능한지 - 완료 시 비활성화
    /// </summary>
    bool CanInteract { get; }

    /// <summary>
    /// 구체적인 상호작용 구현
    /// </summary>
    void Interact();
}
