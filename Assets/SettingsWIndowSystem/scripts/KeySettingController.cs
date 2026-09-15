using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeySettingController : MonoBehaviour
{
    [SerializeField] private KeyBindingName keyBindingName;
    [SerializeField] private TextMeshProUGUI keyButtonText;

    private KeyCode keyBindingValue;

    // 앱 실행 중 한 번만 생성해서 재사용
    private static readonly KeyCode[] AllKeyCodes =
        (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

    private void Start()
    {
        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다!");
            return;
        }

        // Initialize
        keyBindingValue = KeyManager.Instance.GetKeyCode(keyBindingName);
        UpdateTextUI();
    }

    public void SetKeyCode(KeyCode settingKey)
    {
        keyBindingValue = settingKey;
        if(KeyManager.Instance.SetKeyMapping(keyBindingName, keyBindingValue))
        {
            keyBindingValue = settingKey;
            UpdateTextUI();
        }
        else
        {
            // 유저에게 중복 키임을 알림
        }
    }

    public void UpdateTextUI()
    {
        keyButtonText.text = keyBindingValue.ToString();
    }

    /// <summary>
    /// 버튼에서 참조
    /// </summary>
    public void KeySettingHandler()
    {
        StartCoroutine(CoHandleInput());
    }

    private IEnumerator CoHandleInput()
    {
        while (true)
        {
            // 아무 키도 안 눌렸으면 foreach 자체를 스킵
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in AllKeyCodes)
                {
                    if(key >= KeyCode.Mouse0 && key <= KeyCode.Mouse6)
                    {
                        Debug.Log("키 해제");
                        yield break;
                    }
                    else if (Input.GetKeyDown(key))
                    {
                        SetKeyCode(key);
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }
}