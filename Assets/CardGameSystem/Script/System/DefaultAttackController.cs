using System;
using UnityEngine;

public class DefaultAttackController : MonoBehaviour
{

    [Header("Setting")]
    [SerializeField] private int attackAmount;
    [SerializeField] private int blockAmount;

    [Header("Reference")]
    [SerializeField] private PlayerActor playerActor;
    [SerializeField] private OpponentActor opponentActor;

    private Action <int> onTurnStartHandler;

    private void OnEnable()
    {
        onTurnStartHandler = (int _) => { DefaultAttack(_); DefaultAddBlock(_); };
        TurnFlowManager.OnTurnStart += onTurnStartHandler;
    }

    private void OnDisable()
    {
        TurnFlowManager.OnTurnStart -= onTurnStartHandler;
    }

    private void DefaultAttack(int _)
    {
        Debug.Log($"평타 공격 {attackAmount}");
        opponentActor.TakeDamage(attackAmount);
    }

    private void DefaultAddBlock(int _)
    {
        Debug.Log($"평타 방어 {blockAmount}");
        playerActor.AddBlock(blockAmount);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
