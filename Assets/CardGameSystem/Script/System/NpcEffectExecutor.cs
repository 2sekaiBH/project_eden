
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcEffectExecutor : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerActor playerActor;
    [SerializeField] private OpponentActor opponentActor;

    [Header("Reference - CardSelectPanel")]
    [SerializeField] private GameObject cardSelectPanel;

    private NpcData npcData;

    public static event Action OnNpcEffectInProgress;
    public static event Action OnNpcEffectEnd;

    private void OnEnable()
    {
        NpcDisplay.OnNpcSelect += NpcSelectHandler;
    }

    private void OnDisable()
    {
        NpcDisplay.OnNpcSelect -= NpcSelectHandler;
    }

    void Start()
    {

    }

    /// <summary>
    /// Npc 실행 버튼 클릭 시 실행
    /// </summary>
    /// <param name="npc">선택 Npc</param>
    private void NpcSelectHandler(NpcData npc)
    {
        npcData = npc; // npcData 초기화
        StartCoroutine(CoRunNpcEffect());
    }

    private IEnumerator CoRunNpcEffect()
    {
        OnNpcEffectInProgress?.Invoke();

        // npc effect 실행
        NpcContext context = new NpcContext(playerActor, opponentActor, cardSelectPanel);

        yield return StartCoroutine(npcData.effect.ApplyRoutine(context));

        OnNpcEffectEnd?.Invoke();
        yield return null;
    }
}
