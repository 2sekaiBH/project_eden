using System;
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

    [SerializeField] Button applyBtn;

    public NpcData npcData;

    public static event Action<NpcData> OnNpcSelect;

    private Action<int> OnTurnStartHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateNpcDisplay();
    }

    private void OnEnable()
    {
        OnTurnStartHandler = (int _) => { SetInteractableButton(true); };
        TurnFlowManager.OnTurnStart += OnTurnStartHandler;
    }

    private void OnDisable()
    {
        TurnFlowManager.OnTurnStart -= OnTurnStartHandler;
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
    /// npc 선택 버튼에서 관리
    /// </summary>
    public void OnSubmit()
    {
        OnNpcSelect?.Invoke(npcData);// NpcEffectManager에게 선택 npc 데이터 전달
    }


    /// <summary>
    /// npc 적용 버튼 활성화 제어 메소드
    /// turn 시작 시 활성화
    /// 카드 제출 버튼 클릭 시 비활성화(카드 제출 버튼에서 관리)
    /// </summary>
    /// <param name="interactable">상호작용 가능 여부</param>
    public void SetInteractableButton(bool interactable)
    {
        applyBtn.interactable = interactable;
    }

}
