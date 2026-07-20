using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
}
