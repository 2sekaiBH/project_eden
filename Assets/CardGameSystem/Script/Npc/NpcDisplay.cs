using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC slot의 UI 매니저
/// </summary>
public class NpcDisplay : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Image npcProfileImg;
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] TextMeshProUGUI npcEffectText;

    [SerializeField] Button submitButton;

    private NpcData npcData;

    private void OnEnable()
    {
        TurnFlowManager.OnTurnStart += OnTurnStartHandler;
    }

    private void OnDisable()
    {
        TurnFlowManager.OnTurnStart -= OnTurnStartHandler;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    public void SetNpcData(NpcData npcData)
    {
        this.npcData = npcData;
        UpdateNpcDisplay();
    }

    /// <summary>
    /// Npc Slot UI 반영
    /// </summary>
    private void UpdateNpcDisplay()
    {
        npcProfileImg.sprite = npcData.npcProfileImage;
        npcNameText.text = npcData.name;
        npcEffectText.text = npcData.effectDescription;
    }

    /// <summary>
    /// Turn 시작 시 npc 사용 버튼 활성화
    /// </summary>
    /// <param name="_"></param>
    private void OnTurnStartHandler(int _)
    {
        SetInteractableButton(true);
    }

    /// <summary>
    /// 플레이어 cardSubmit 시 비활성화
    /// 카드 submitButton에서 제어
    /// </summary>
    public void OnSelectEndFlag()
    {
        SetInteractableButton(false);
    }

    public void OnSubmit()
    {
        // NpcEffectManager에게 선택 npc 데이터 전달
    }

    private void SetInteractableButton(bool interactable)
    {
        submitButton.interactable = interactable;
    }
}
