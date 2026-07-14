using UnityEngine;

/// <summary>
/// 카드를 사용하는 주체(플레이어, 보스 등)가 구현해야 하는 최소 인터페이스.
/// CardEffect들은 구체 클래스가 아니라 이 인터페이스에만 의존한다.
/// </summary>
public interface ICardActor
{
   int CurrentHp { get; }
    int CurrentBlock { get; }

    void TakeDamage(int amount);
    void AddBlock(int amount);
    void DrawCards(int count);
}
