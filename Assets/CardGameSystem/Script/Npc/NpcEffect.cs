using UnityEngine;

/// <summary>
/// Npc의 효과를 정의하는 추상클래스
/// </summary>
public abstract class NpcEffect : ScriptableObject
{
    public abstract void Apply();
}
