using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIUpdator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private List<TextMeshProUGUI> stateDescriptionText = new List<TextMeshProUGUI>(MaxTextCount);
    [SerializeField] private RectTransform rectTransform;

    private static UIUpdator instance;
    public static UIUpdator Instance => instance;
    private const int MaxTextCount = 3;

    private Queue<(string, CasterType)> textQueue = new Queue<(string, CasterType)>(MaxTextCount);

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        if (rectTransform == null) 
            rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string text, CasterType caster = CasterType.System)
    {
        // 최대 3개만 유지
        if (textQueue.Count >= MaxTextCount)
        {
            textQueue.Dequeue();
        }

        textQueue.Enqueue(( text, caster ));

        UpdateCurrentTextUI();
    }


    Color textColor;
    private void UpdateCurrentTextUI()
    {
        int index = 0;

        foreach (var item in textQueue)
        {
            switch (item.Item2)
            {
                case (CasterType.Player):
                    textColor = new Color(102f / 255f, 1f, 1f); //플레이어일 시 파란색 text로 표시
                    break;
                case (CasterType.Opponent):
                    textColor = new Color(1f, 152f / 255f, 1f);
                    break;
                case (CasterType.System):
                    textColor = new Color(1f, 1f, 1f);
                    break;
            }

            stateDescriptionText[index].color = textColor;
            stateDescriptionText[index].text = item.Item1;
            
            index++;
        }

        while (index < MaxTextCount)
        {
            stateDescriptionText[index++].text = "";
        }
    }
}


public enum CasterType
{
    Player,
    Opponent,
    System
}
