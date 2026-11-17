using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject saveSlotsPanel;

    [Header("Scene")]
    [SerializeField] private string introSceneName = "00_IntroScene";

    [Header("Menu Intro")]
    [SerializeField] private CanvasGroup menuButtonsGroup;
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private float menuAppearDelay = 3f;
    [SerializeField] private float menuFadeDuration = 1f;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (saveSlotsPanel != null)
            saveSlotsPanel.SetActive(false);

        if (menuButtonsGroup != null)
        {
            menuButtonsGroup.alpha = 0f;
            menuButtonsGroup.interactable = false;
            menuButtonsGroup.blocksRaycasts = false;

            StartCoroutine(ShowMenuButtons());
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(introSceneName);
    }

    public void OpenOptions()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSaveSlots()
    {
        if (saveSlotsPanel != null)
            saveSlotsPanel.SetActive(true);
    }

    public void CloseSaveSlots()
    {
        if (saveSlotsPanel != null)
            saveSlotsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator ShowMenuButtons()
    {
        yield return new WaitForSecondsRealtime(menuAppearDelay);

        float elapsed = 0f;

        while (elapsed < menuFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(elapsed / menuFadeDuration);

            menuButtonsGroup.alpha = t;

            yield return null;
        }

        menuButtonsGroup.alpha = 1f;
        menuButtonsGroup.interactable = true;
        menuButtonsGroup.blocksRaycasts = true;

        if (EventSystem.current != null &&
            firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(
                firstSelectedButton
            );
        }
    }
}