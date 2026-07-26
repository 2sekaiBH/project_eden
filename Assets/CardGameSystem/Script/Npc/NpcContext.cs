using UnityEngine;

public class NpcContext
{
    public PlayerActor playerActor { get; }
    public OpponentActor opponentActor { get; }
    public GameObject gameObject { get; }

    /// <summary>
    /// CardExecute 시 필요한 정보
    /// </summary>
    /// <param name="playerActor">플레이어 Actor</param>
    /// <param name="opponentActor">상대 Actor</param>
    public NpcContext(PlayerActor playerActor, OpponentActor opponentActor, GameObject gameObject)
    {
        this.playerActor = playerActor;
        this.opponentActor = opponentActor;
        this.gameObject = gameObject;
    }
}
