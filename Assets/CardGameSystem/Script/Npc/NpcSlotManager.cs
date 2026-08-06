using System.Collections.Generic;
using UnityEngine;

public class NpcSlotManager : MonoBehaviour
{
    [SerializeField] private List<NpcData> npcSlotList = new List<NpcData>();
    public List<NpcData> NpcSlotList => npcSlotList;

    private List<NpcDisplay> npcDisplays = new List<NpcDisplay>();
    private List<GameObject> npcObj = new List<GameObject>();

    [Header("Setting")]
    [SerializeField] private GameObject npcSlotPrefab;

    [Header("Reference")]
    [SerializeField] private RectTransform rectTransform;

    private void Awake()
    {
        if(rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        
    }

    public void Initialize(List<NpcData> npcDatas)
    {
        foreach (var npcData in npcDatas)
        {
            GameObject newNpcObj = Instantiate(npcSlotPrefab, rectTransform, false);
            npcObj.Add(newNpcObj);
            NpcDisplay newDisplay = newNpcObj.GetComponent<NpcDisplay>();
            npcDisplays.Add(newDisplay);

            newDisplay.SetNpcData(npcData);
        }
    }
}