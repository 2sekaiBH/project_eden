using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject settingsWIndowCanvas;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
    {
        OpenCloseSettingsWindow.OnOpenCloseSettings += OpenCloseSettingsWIndow;
    }

    private void OnDisable()
    {
        OpenCloseSettingsWindow.OnOpenCloseSettings -= OpenCloseSettingsWIndow;
    }



    private void Awake()
    {
        if(SoundManager.Instance == null)
        {
            Debug.LogWarning("SoundManager가 없습니다!");
            return;
        }

        Time.timeScale = 0f; // 일시 정지

        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSfxVolume);

        bgmSlider.value = SoundManager.Instance.GetBgmVolume();
        sfxSlider.value = SoundManager.Instance.GetSfxVolume();
    }

    public void OpenCloseSettingsWIndow(bool isOpen)
    {
        if (isOpen)
        {
            Time.timeScale = 0f; // 설정창이 열릴 때 시간 정지
            if (settingsWIndowCanvas != null) settingsWIndowCanvas.SetActive(true);
        }
        else
        {
            CloseSettings(); // 닫기 요청 시 CloseSettings 호출
        }
    }

    public void CloseSettings()
    {
        Time.timeScale = 1f; // 일시 정지 해제

        // SettingsScene을 메모리에서 완전히 Unload시켜야 다음에 다시 열 수 있습니다.
        Scene settingsScene = SceneManager.GetSceneByName("SettingsScene");
        if (settingsScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync("SettingsScene");
        }
        else if (settingsWIndowCanvas != null)
        {
            settingsWIndowCanvas.SetActive(false);
        }
    }

}
