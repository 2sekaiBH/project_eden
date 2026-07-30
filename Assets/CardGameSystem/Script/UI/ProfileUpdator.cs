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

    [Header("UI Updator Reference")]
    [SerializeField] private HpBlockUIUpdator hpBlockUIUpdator;

    [Header("Data")]
    [SerializeField] private Sprite[] profileImgDatas;

    private int maxHp;
    public void UpdateProfile(string name, int hp, int block, int energy = -1)
    {
        nameUI.text = name;
        hpUI.text = $"<sprite=1> <color=#EB4F51><b>{hp}</b></color><color=#DE7D82>/<size=60%>{maxHp}</size></color> <sprite=2> <color=#55C1FF>{block}</color>";
        if(energyUI != null)
            energyUI.text = $"energy: {energy}";

        hpBlockUIUpdator.RefreshAnimated(hp, block);

        //profileImg.sprite = profileImgDatas[CardGameManager.Instance.stage + 1]
    }

    public void InitializeUpdator(int maxHp, int maxEnergy)
    {
        this.maxHp = maxHp;

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
