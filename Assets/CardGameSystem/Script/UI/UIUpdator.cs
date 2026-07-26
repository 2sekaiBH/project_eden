using TMPro;
using UnityEngine;

public class UIUpdator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    private static UIUpdator instance;
    public static UIUpdator Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void SetText(string text)
    {
        textUI.text = text;
    }
}
