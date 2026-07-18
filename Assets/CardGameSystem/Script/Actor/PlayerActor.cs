using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerActor : Actor
{
    [Header("Setting")]
    [SerializeField] private int maxEnergy;
    [SerializeField] private int maxHp;

    private int currentEnergy = 4;
    public int CurrentEnergy => currentEnergy;

    public override void Initialize()
    {
        currentHp = maxHp;
        currentBlock = 0;
        currentEnergy = maxEnergy;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
        name = "Player"; // 디버깅용 - 게임 매니저랑 연결
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, currentEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
