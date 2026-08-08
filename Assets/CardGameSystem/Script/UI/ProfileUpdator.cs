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

    public void UpdateProfileImg(string name)
    {
        switch(name)
        {
            case ("행복 좀비"):
                Debug.Log("행복 좀비 프로필 갱신");
                profileImg.sprite = profileImgDatas[0];
                break;
            case ("생체 실험 폐기물"):
                Debug.Log("생체 실험 폐기물 프로필 갱신");
                profileImg.sprite = profileImgDatas[1];
                break;
            case ("데이터 포식자"):
                Debug.Log("데이터 포식자 프로필 갱신");
                profileImg.sprite = profileImgDatas[2];
                break;
            case ("이브"):
                Debug.Log("이브 프로필 갱신");
                profileImg.sprite = profileImgDatas[3];
                break;
            case ("아키텍트"):
                Debug.Log("아키텍트 프로필 갱신");
                profileImg.sprite = profileImgDatas[4];
                break;
            default:
                Debug.LogWarning("맞는 프로필 스프라이트가 없습니다.");
                break;
        }
    }
}
