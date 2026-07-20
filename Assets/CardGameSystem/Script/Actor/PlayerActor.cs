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
        handManager.StartSelect(hand, this);
    }

    void Start()
    {
        Initialize();
        name = "Player"; // 디버깅용 - 커스텀 name으로 변경
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
