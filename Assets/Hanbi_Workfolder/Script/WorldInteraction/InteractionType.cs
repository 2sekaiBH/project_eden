using UnityEngine;

/// <summary>
/// 상호작용 타입
/// </summary>
public enum InteractionType
{
   None,
    /// <summary>
    /// npc와의 상호작용
    /// </summary>
    NPC,
    /// <summary>
    /// 아이템 관련 상호작용(아이템 사용, 획득)
    /// </summary>
    UseItem,
    /// <summary>
    /// 씬 이동 상호작용
    /// </summary>
    Portal,
}
