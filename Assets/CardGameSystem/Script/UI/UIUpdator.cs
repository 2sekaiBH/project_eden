using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private List<TextMeshProUGUI> stateDescriptionText = new List<TextMeshProUGUI>(MaxTextCount);
    [SerializeField] private RectTransform rectTransform;

    private static UIUpdator instance;
    public static UIUpdator Instance => instance;
    private const int MaxTextCount = 3;

    private Queue<string> textQueue = new Queue<string>(MaxTextCount);

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        if (rectTransform == null) 
            rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string text)
    {
        // 최대 3개만 유지
        if (textQueue.Count >= MaxTextCount)
        {
            textQueue.Dequeue();
        }

        textQueue.Enqueue(text);

        UpdateCurrentTextUI();
    }

    private void UpdateCurrentTextUI()
    {
        int index = 0;

        foreach (string text in textQueue)
        {
            stateDescriptionText[index++].text = text;
        }

        while (index < MaxTextCount)
        {
            stateDescriptionText[index++].text = "";
        }
    }
}
