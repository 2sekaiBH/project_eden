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
    [SerializeField] private List<NpcData> npcDataList; // database 참조 형식으로 개선 필요

    [Header("DataBase")]
    // [SerializeField] private OpponentData opponentData;
    // [SerializeField] private NpcDataBase npcDataBase; // 데이터 베이스 참조 후 전달 기능 추후 추가
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
    void Start()
    {
        /*
        // 게임 전역 데이터 초기화
        InitializeGameData();

        // 게임 실행
        RunCardGame();
        */

    }

    /// <summary>
    /// 외부에서 호출하는 카드 게임 시작 함수
    /// </summary>
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

    /*
    private void RunCardGame()
    {
        roundFlowManager.StartRound();
    }
    */

    private void HandleCardGameResult(bool result)
    {
        if (result)
        {
            Debug.Log("카드 게임 승리");
            onGameCleared?.Invoke();
        }
        else
        {
            Debug.Log("카드 게임 패배");
            onGameFailed?.Invoke();
        }
    }

    /*
    // 최종 승패 판정에 따른 처리
    private void HandleCardGameResult(bool result)
    {
        if(result == true) // 승리 시
        {
            StageType? nextStage = GetNextStage(stage);

            Debug.Log($"다음 스테이지: {nextStage}");

            if (nextStage.HasValue)
            {
                // GameManager.Instance.SetLastStage(nextStage.Value); // 다음 스테이지 정보 갱신 (아래 참고)
                SceneManager.LoadScene(nextStage.Value.ToString());
            }
            else
            {
                // 마지막 스테이지 클리어 -> 엔딩/클리어 씬 등으로 이동
                Debug.Log("엔딩으로 이동합니다.");
                // SceneManager.LoadScene("EndingScene"); // 실제 씬 이름으로 교체
            }
        }
        else // 패배 시
        {
            // 필요하다면 패배 처리도 여기에 (재시도 씬, 게임오버 씬 등)
        }
    }

    /// <summary>
    /// 현재 stage의 다음 StageType을 반환. 마지막이면 null.
    /// </summary>
    private StageType? GetNextStage(StageType current)
    {
        StageType[] allStages = (StageType[])System.Enum.GetValues(typeof(StageType));
        int currentIndex = System.Array.IndexOf(allStages, current);

        if (currentIndex == -1)
        {
            Debug.LogWarning($"StageType에 없는 값입니다: {current}");
            return null;
        }

        int nextIndex = currentIndex + 1;
        if (nextIndex >= allStages.Length)
            return null; // 마지막 스테이지였음

        return allStages[nextIndex];
    }
    */
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
