using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;


public class MissionProgress
{
    public bool tookDamage = false;
    public int usedCardCount = 0;
    public bool usedOnlyOneE = true;


    public bool complete;
}

// 미션 종류
public enum MissionType
{
    NoDamage,  //이번 턴 피해X
    Use3Cards, //이번 턴 카드 3개 이상 사용
    OnlyOneCard //이번 턴 오직 1코스트 카드만 사용
}

public class MissionManager : MonoBehaviour
{
    //MissionManager를 싱글톤으로 만듦
    public static MissionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private MissionType mission;

    private MissionProgress playerProgress;
    private MissionProgress opponentProgress;

    public MissionType Mission => mission;

    [SerializeField] private TMP_Text missionText; //미션 내용을 띄울 텍스트, 성공 시 이 부분을 달성! 으로 바꿈


    //시작할 때 새롭게 미션을 생성하고 진행도 저장할 객체를 만듦
    public void GenerateMission()
    {
        playerProgress = new MissionProgress();
        opponentProgress = new MissionProgress();

        mission = (MissionType)Random.Range(0, 3); //미션을 랜덤으로 뽑는 함수
        Debug.Log($"미션 이거 뽑혔음 {mission}");
        missionText.text = GetMissionText();
    }

    //출력할 미션 내용 텍스트
    public string GetMissionText()
    {
        switch (mission)
        {
            case MissionType.NoDamage:
                return "이번 턴에 피해 받지 않기";

            case MissionType.Use3Cards:
                return "이번 턴에 카드 3장 이상 사용하기";

            case MissionType.OnlyOneCard:
                return "이번 턴에 1E 카드만 사용하기";

            default:
                return "";

        }
    }



    //저장할 대상이 Player인지 Opponent인지 찾아주는 함수
    private MissionProgress GetProgress(Actor actor)
    {
        if (actor is PlayerActor)
            return playerProgress;

        else
            return opponentProgress;
    }


    //NoDamage 미션, 데미지를 받을 경우 활성화
    public void TakeDamage(Actor actor)
    {
        MissionProgress progress = GetProgress(actor); //일단 누구의 진행도인지 가져옴
        Debug.Log($"{actor}의 진행도");
        progress.tookDamage = true;

    }
    
    //이번 턴에 3장 이상의 카드 사용 미션, 카드 사용 저장 변수의 값을 올림
    public void UseCard(Actor actor, CardData card)
    {
        MissionProgress progress = GetProgress(actor); //일단 누구의 진행도인지 가져옴
        Debug.Log($"{actor}의 진행도");
        progress.usedCardCount++;

        if (card.energyCost > 1) //사용한 카드가 1코스트인지 확인
            progress.usedOnlyOneE = false; //1코스트가 아닐 경우 false로 값을 바꿔줌
    }
    



    //미션 성공 여부 판단

public bool Check(MissionProgress progress)
    {
        switch(mission)
        {
            case MissionType.NoDamage:
                return !progress.tookDamage;
                
           case MissionType.Use3Cards:
                return progress.usedCardCount >= 3;
                
           case MissionType.OnlyOneCard:
                return progress.usedOnlyOneE;
                
            default:
                return false;
        
        }
    }


    //플레이어와 보스의 성공 여부를 저장
    public void EvaluateMission()
    {
        playerProgress.complete = Check(playerProgress);
        opponentProgress.complete = Check(opponentProgress);

        if (playerProgress.complete)
        {
            missionText.text = "달성!"; //플레이어가 미션 성공 시 미션 내용 텍스트 내용을 달성으로 바꿈
        }

        else
            missionText.text = "실패";

        Debug.Log($" 플레이어는 {playerProgress.complete}, 몹은 {opponentProgress.complete}");
    }
    
    //플레이어와 보스의 성공 여부 리턴

    public bool IsMissionComplete(Actor actor)
    {
        return GetProgress(actor).complete;
    }



}