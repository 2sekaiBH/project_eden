using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ���� �Ŵ���
/// �������� ���� �ݿ�
/// ���� ó��
/// </summary>
public class CardGameManager : MonoBehaviour
{
    // [SerializeField] private List<NpcData> npcDataList; // database ���� �������� ���� �ʿ�

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

    public UnityEvent OnGameCleared => onGameCleared;
    public UnityEvent OnGameFailed => onGameFailed;

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
            roundFlowManager = GetComponentInChildren<RoundFlowManager>();
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
                    Debug.LogWarning("GameState ����!, �⺻ �̸� player�� ��ü");
                    playerActor.SetPlayer("Player", stageData.playerMaxHp);
                }
                else
                {
                    playerActor.SetPlayer(GameState.Instance.PlayerName, stageData.playerMaxHp);
                }
            }
        }
        // Debug.LogWarning("�ʱ�ȭ�� �������� ������ �����ϴ�.");
    }

    // 최종 승패 판정에 따른 처리
    private void HandleCardGameResult(bool result)
    {
        if (result)
        {
            Debug.Log("[CardGame] 카드게임 승리");
            onGameCleared?.Invoke();
            return;
        }

        Debug.Log("[CardGame] 카드게임 패배 → GameOver 엔딩으로 이동");

        if (GameState.Instance == null)
        {
            Debug.LogError(
                "[CardGame] GameState가 없어 GameOver 엔딩을 설정할 수 없습니다."
            );
            return;
        }

        // EndingDialogueStarter가 이 값을 읽고
        // ending_gameover_001부터 재생하게 됨
        GameState.Instance.SetSelectedEnding(EndingType.GameOver);

        // 기존 Inspector 이벤트가 필요하다면 먼저 실행
        onGameFailed?.Invoke();

        // 동일한 엔딩 씬으로 이동
        SceneManager.LoadScene("05_EndingScene");
    }

}


[System.Serializable]
public class StageData
{
    public string stageName;
    public int playerMaxHp;
    public List<NpcData> joinNpc;
    public OpponentData opponent;
}
