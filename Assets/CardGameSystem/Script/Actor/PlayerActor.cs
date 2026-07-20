using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerActor : Actor
{
    [Header("Reference")]
    [SerializeField] private HandManager handManager;
    public HandManager HandManager => handManager;

    [Header("Setting")]
    [SerializeField] private int maxEnergy;
    [SerializeField] private int maxHp;

    public override void Initialize()
    {
        currentHp = maxHp;
        currentBlock = 0;
        currentEnergy = maxEnergy;
    }

    // 카드 선택 시작
    public override void SelectCard()
    {
        handManager.StartSelect(hand);
    }

    void Start()
    {
        Initialize();
        name = "Player"; // 디버깅용 - 게임 매니저랑 연결, 커스텀 name으로 변경
        UpdateProfileUI();
    }

    public override void UpdateProfileUI()
    {
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, currentEnergy);
    }
}
