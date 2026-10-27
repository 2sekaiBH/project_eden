using System;
using UnityEngine;

public class OpenCloseSettingsWindow : MonoBehaviour
{
    private KeyCode SettingsKeyCode = KeyCode.Escape;
    public static event Action<bool> OnOpenCloseSettings;
    public static Func<bool> OnEscPressed;
    private static bool isOpen;

    private void OnDisable()
    {
        if (KeyManager.Instance == null) return;
        KeyManager.Instance.OnKeyChanged -= UpdateKeyCode;
    }

    private void Start()
    {
        isOpen = false;
        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다! - 기본 Esc 키를 사용합니다.");
            return;
        }

        KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
        UpdateKeyCode();
    }

    private void UpdateKeyCode()
    {
        SettingsKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.Settings);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(SettingsKeyCode)) return;

        if (OnEscPressed != null)
        {
            bool wasHandled = false;
            foreach (Func<bool> handler in OnEscPressed.GetInvocationList())
            {
                if (handler.Invoke()) wasHandled = true;
            }

            if (wasHandled) return;
        }

        SetOpen(!isOpen);
    }

    public static void SetOpen(bool open)
    {
        isOpen = open;
        OnOpenCloseSettings?.Invoke(isOpen);
    }
}
