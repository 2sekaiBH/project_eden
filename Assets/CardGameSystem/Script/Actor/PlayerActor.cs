using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 플레이어 행동 제어 스크립트
/// </summary>
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
        handManager.StartSelect(hand, this);
    }

    void Awake()
    {
        name = "Player"; // 디버깅용 - 커스텀 name으로 변경
        Initialize();
        UpdateProfileUI();
    }

    public override void UpdateProfileUI()
    {
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, currentEnergy);
    }

    public override void EnergyIntialize()
    {
        SetEnergy(maxEnergy);
    }
}
