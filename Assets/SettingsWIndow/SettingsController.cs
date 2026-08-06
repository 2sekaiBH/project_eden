using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
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

    private void OnDisable()
    {
        Time.timeScale = 1f; // 일시 정지 해제
    }

    public void CloseSettingsScene()
    {
        Scene targetScene = SceneManager.GetSceneByName("SettingsScene");
        if (targetScene.IsValid() && targetScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(targetScene);
        }
    }
}
