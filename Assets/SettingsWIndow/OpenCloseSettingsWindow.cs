using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenCloseSettingsWindow : MonoBehaviour
{
    private KeyCode SettingsKeyCode = KeyCode.Escape;
    public static event Action<bool> OnOpenCloseSettings;

    public static Func<bool> OnEscPressed;

    private void OnDisable()
    {
        if (KeyManager.Instance == null) return;
        KeyManager.Instance.OnKeyChanged -= UpdateKeyCode;
    }

    private void Start()
    {
        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다! - 기본 Esc로 KeyCode 설정");
            return;
        }

        KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
        UpdateKeyCode();
    }

    private void UpdateKeyCode()
    {
        SettingsKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.Settings);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(SettingsKeyCode))
        {
            if (OnEscPressed != null)
            {
                bool wasHandled = false;
                foreach (Func<bool> handler in OnEscPressed.GetInvocationList())
                {
                    if (handler.Invoke())
                    {
                        wasHandled = true;
                    }
                }

                if (wasHandled) return;
            }

            if (!SceneManager.GetSceneByName("SettingsScene").isLoaded)
            {
                SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive);
            }
        }
    }
}
