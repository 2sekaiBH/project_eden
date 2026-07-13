using UnityEngine;

/// <summary>
/// 상호작용 타입
/// </summary>
public enum InteractionType
{
    /// <summary>
    /// 다이얼로그 상호작용
    /// </summary>
    Dialog,
    /// <summary>
    /// 아이템 관련 상호작용(아이템 획득)
    /// </summary>
    Item,
    /// <summary>
    /// 팝업 상호작용(관련 일러스트 팝업)
    /// </summary>
    PopUp,
    /// <summary>
    /// 퍼즐 상호작용
    /// </summary>
    Puzzle,
    /// <summary>
    /// 씬(스테이지) 이동 상호작용
    /// </summary>
    Portal,
    /// <summary>
    /// 기타 상호작용
    /// </summary>
    Other
}
