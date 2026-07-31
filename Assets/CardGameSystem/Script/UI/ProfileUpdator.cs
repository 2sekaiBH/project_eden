using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Actor의 UI를 갱신하는 스크립트
/// </summary>
public class ProfileUpdator : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Image profileImg;
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI hpUI;
    [SerializeField] private TextMeshProUGUI energyUI = null;
    [SerializeField] private TextMeshProUGUI maxEnergyUI = null;

    [Header("UI Updator Reference")]
    [SerializeField] private HpBlockUIUpdator hpBlockUIUpdator;

    [Header("Data")]
    [SerializeField] private Sprite[] profileImgDatas;

    private int maxHp;
    private int maxEnergy;

    public void UpdateProfile(string name, int hp, int block, int energy = -1)
    {
        nameUI.text = name;
        hpUI.text = $"<sprite=1><b>{hp}</b>/<size=60%>{maxHp}</size>   <sprite=2> {block}";
        if(energyUI != null)
            energyUI.text = $"{energy}";

        hpBlockUIUpdator.RefreshAnimated(hp, block);

        //profileImg.sprite = profileImgDatas[CardGameManager.Instance.stage + 1]
    }

    public void InitializeUpdator(int maxHp = -1, int maxEnergy = -1)
    {
        if(maxHp != -1)
            this.maxHp = maxHp;

        if (maxEnergy != -1)
            this.maxEnergy = maxEnergy;

        if (maxEnergyUI != null)
            maxEnergyUI.text = $"{maxEnergy}";

        hpBlockUIUpdator.Initialize(maxHp);
    }

    /// <summary>
    /// actor의 활성화 - 비활성화 상태를 UI에 반영
    /// </summary>
    /// <param name="active"></param>
    public void UpdateActiveProfile(bool active)
    {
        profileImg.color = active ? new Color(1, 1, 1, 1f) :new Color(1, 1, 1, 0.5f);
    }
}
