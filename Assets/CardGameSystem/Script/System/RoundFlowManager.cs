using System.Collections;
using UnityEngine;

public class RoundFlowManager : MonoBehaviour
{
    [Header("Round Setting")]
    [SerializeField] private int turrnsPerRound = 2;
    [SerializeField] private int maxRounds = 5;

    private int currentRound = 0;
    public int CurrentRound => currentRound;

    private TurnFlowManager turnFlowManager;

    private Coroutine coRunTurn; // 중복 검사 로직 추가

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        turnFlowManager = GetComponent<TurnFlowManager>();
    }

    void Start()
    {
        StartCoroutine(RunRound()); // 디버깅용
    }
    
    public IEnumerator RunRound()
    {
        //while(currentRound < maxRounds)
        {
            // 1. 라운드 시작
            currentRound++;

            // 2. 턴 시작
            yield return StartCoroutine(turnFlowManager.RunTurn());

            // 3. 라운드 계산
        }

        // start gameover coroutine
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
