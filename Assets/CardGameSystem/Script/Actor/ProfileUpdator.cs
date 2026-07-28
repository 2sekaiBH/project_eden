using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Actor의 UI를 갱신하는 스크립트
/// </summary>
public class ProfileUpdator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Image profileImg;
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI hpUI;
    [SerializeField] private TextMeshProUGUI energyUI = null;

    [Header("Data")]
    [SerializeField] private Sprite[] profileImgDatas;

    public void UpdateProfile(string name, int hp, int block, int energy = -1)
    {
        nameUI.text = name;
        hpUI.text = $"hp: {hp}, block: {block}";
        if(energyUI != null)
            energyUI.text = $"energy: {energy}";

        //profileImg.sprite = profileImgDatas[CardGameManager.Instance.stage + 1]
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
