using UnityEngine;

/// <summary>
/// 상위 게임 매니저
/// 스테이지 정보 반영
/// 승패 처리
/// </summary>
public class CardGameManager : MonoBehaviour
{
    private RoundFlowManager roundFlowManager;
    private bool isWIn = false;

    private void Awake()
    {
        roundFlowManager = GetComponentInChildren<RoundFlowManager>();
    }
    void Start()
    {
        RunCardGame();
    }

    private void RunCardGame()
    {
        roundFlowManager.StartRound();
    }

    // 최종 승패 판정에 따른 처리
}
