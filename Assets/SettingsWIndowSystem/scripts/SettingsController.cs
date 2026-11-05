using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject settingsWIndowCanvas;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private KeyCode settingsKeyCode = KeyCode.Escape;
    private bool isOpen = false;

    private void OnEnable()
    {
        OpenCloseSettingsWindow.OnOpenCloseSettings += OpenCloseSettingsWIndow;
        if (KeyManager.Instance != null)
        {
            KeyManager.Instance.OnKeyChanged += UpdateSettingsKeyCode;
            UpdateSettingsKeyCode();
        }
    }

    private void OnDisable()
    {
        OpenCloseSettingsWindow.OnOpenCloseSettings -= OpenCloseSettingsWIndow;
        if (KeyManager.Instance != null)
        {
            KeyManager.Instance.OnKeyChanged -= UpdateSettingsKeyCode;
        }
    }

    private void Awake()
    {
        if (settingsWIndowCanvas != null)
        {
            settingsWIndowCanvas.SetActive(false);
        }

        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("SoundManager가 없습니다!");
            return;
        }

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBgmVolume);
            bgmSlider.SetValueWithoutNotify(SoundManager.Instance.GetBgmVolume());
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSfxVolume);
            sfxSlider.SetValueWithoutNotify(SoundManager.Instance.GetSfxVolume());
        }
    }

    private void Update()
    {
        if (settingsKeyCode == KeyCode.None || !Input.GetKeyDown(settingsKeyCode))
        {
            return;
        }

        if (OpenCloseSettingsWindow.OnEscPressed != null)
        {
            bool wasHandled = false;
            foreach (Func<bool> handler in OpenCloseSettingsWindow.OnEscPressed.GetInvocationList())
            {
                if (handler.Invoke()) wasHandled = true;
            }

            if (wasHandled) return;
        }

        OpenCloseSettingsWindow.SetOpen(!isOpen);
    }

    private void UpdateSettingsKeyCode()
    {
        if (KeyManager.Instance != null)
        {
            settingsKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.Settings);
        }
    }

    public void OpenCloseSettingsWIndow(bool open)
    {
        SetSettingsOpen(open);
    }

    private void SetSettingsOpen(bool open)
    {
        isOpen = open;
        if (settingsWIndowCanvas != null)
        {
            settingsWIndowCanvas.SetActive(isOpen);
        }

        Time.timeScale = isOpen ? 0f : 1f;
    }

    public void CloseSettings()
    {
        OpenCloseSettingsWindow.SetOpen(false);
    }

    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
