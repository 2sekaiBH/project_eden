using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상위 게임 매니저
/// 스테이지 정보 반영
/// 승패 처리
/// </summary>
public class CardGameManager : MonoBehaviour
{
    [SerializeField] private List<NpcData> npcDataList; // database 참조 형식으로 개선 필요

    [Header("DataBase")]
    [SerializeField] private OpponentData opponentData;
    [SerializeField] private NpcDataBase npcDataBase; // 데이터 베이스 참조 후 전달 기능 추후 추가

    [Header("Other Managers")]
    [SerializeField] private RoundFlowManager roundFlowManager;
    [SerializeField] private NpcSlotManager npcSlotManager;

    [SerializeField]
    private List<StageData> stageDataList = new List<StageData>();

    private bool isWIn = false;

    private void Awake()
    {
        if (roundFlowManager == null)
            GetComponentInChildren<RoundFlowManager>();
    }
    void Start()
    {
        // 게임 전역 데이터 초기화
        InitializeGameData();

        // 게임 실행
        RunCardGame();
    }

    void InitializeGameData()
    {
        foreach(var stageData in stageDataList)
        {
            if(stageData.stageName.Equals(GameManager.Instance.LastStage))
            {
                npcSlotManager.Initialize(stageData.joinNpc);
                // OpponentActor에게 opponentData 전달
            }
        }
    }

    private void RunCardGame()
    {
        roundFlowManager.StartRound();
    }

    // 최종 승패 판정에 따른 처리
}

/// <summary>
/// 1,2,3 스테이지별 중간보스, npcSlot 데이터
/// </summary>
[System.Serializable]
public class StageData
{
    public string stageName;
    public List<NpcData> joinNpc;
    public OpponentActor opponent;
}
