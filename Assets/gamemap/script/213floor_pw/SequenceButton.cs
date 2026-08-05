using UnityEngine;

public class SequenceButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("이 버튼의 고유 ID 번호")]
    [SerializeField] private int buttonId;

    public void OnClickUIButton()
    {
        PressButton();
    }

    public void PressButton()
    {
        if (ButtonSequenceManager.Instance != null)
        {
            ButtonSequenceManager.Instance.OnPressButton(buttonId);
        }
        else
        {
            Debug.LogError("ButtonSequenceManager.Instance를 찾을 수 없습니다!");
        }
    }
}
