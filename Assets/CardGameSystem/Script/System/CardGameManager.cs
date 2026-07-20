using UnityEngine;

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
