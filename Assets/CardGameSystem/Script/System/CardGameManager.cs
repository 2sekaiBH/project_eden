using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 상위 게임 매니저
/// 스테이지 정보 반영
/// 승패 처리
/// </summary>
public class CardGameManager : MonoBehaviour
{
    // [SerializeField] private List<NpcData> npcDataList; // database 참조 형식으로 개선 필요

    [Header("DataBase")]
    [SerializeField] private List<StageData> stageDataList = new List<StageData>();

    [Header("Other Managers")]
    [SerializeField] private RoundFlowManager roundFlowManager;
    [SerializeField] private NpcSlotManager npcSlotManager;
    [SerializeField] private PlayerActor playerActor;
    [SerializeField] private OpponentActor opponentActor;

    [Header("Result Events")]
    [SerializeField] private UnityEvent onGameCleared;
    [SerializeField] private UnityEvent onGameFailed;

    // private bool isWIn = false;

    private StageType stage;

    private void OnEnable()
    {
        roundFlowManager.OnResultDetermined += HandleCardGameResult;
    }

    private void OnDisable()
    {
        roundFlowManager.OnResultDetermined -= HandleCardGameResult;
    }

    private void Awake()
    {
        if (roundFlowManager == null)
            GetComponentInChildren<RoundFlowManager>();
    }

    public void StartCardGame()
    {
        InitializeGameData();
        roundFlowManager.StartRound();
    }

    void InitializeGameData()
    {
        stage = GameManager.Instance.LastStage;

        foreach (var stageData in stageDataList)
        {
            if (stageData.stageName.Equals(stage.ToString()))
            {
                npcSlotManager.Initialize(stageData.joinNpc);
                opponentActor.SetOpponent(stageData.opponent);
                if (GameState.Instance == null)
                {
                    Debug.LogWarning("GameState 없음!, 기본 이름 player로 대체");
                    playerActor.SetPlayer("Player", stageData.playerMaxHp);
                }
                else
                {
                    playerActor.SetPlayer(GameState.Instance.PlayerName, stageData.playerMaxHp);
                }
            }
        }
        // Debug.LogWarning("초기화할 스테이지 정보가 없습니다.");
    }

    // 최종 승패 판정에 따른 처리
    private void HandleCardGameResult(bool result)
    {
        if(result)
        {
            Debug.Log("");
            onGameCleared?.Invoke();
        }
        else{
            Debug.Log("");
            onGameFailed?.Invoke();
        }
    }
}


/// <summary>
/// 1,2,3 스테이지별 중간보스, npcSlot 데이터
/// </summary>
[System.Serializable]
public class StageData
{
    public string stageName;
    public int playerMaxHp;
    public List<NpcData> joinNpc;
    public OpponentData opponent;
}
