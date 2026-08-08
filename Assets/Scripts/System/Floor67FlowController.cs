using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Floor67FlowController : MonoBehaviour
{
    private enum FlowState
    {
        Exploring,
        PreCardDialogue,
        CardGame,
        PostCardDialogue,
        ReadyToExit
    }

    [Header("Dialogue")]
    [SerializeField] private IntroDialogueController dialogueController;

    [Header("Card Game")]
    [SerializeField] private UnityEvent onCardGameStart;

    [Header("Exit")]
    [SerializeField] private GameObject endTrigger;

    [Header("Next Floor")]
    [SerializeField] private string nextSceneName;

    [Header("Loading")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private float minimumLoadingTime = 1.2f;

    private FlowState state = FlowState.Exploring;

    private void Start()
    {
        if (endTrigger != null)
            endTrigger.SetActive(false);
        if (loadingCanvas != null)
            loadingCanvas.SetActive(false);
    }

    // 자물쇠 성공
    public void OnLockCleared()
    {
        if (state != FlowState.Exploring)
            return;

        state = FlowState.PreCardDialogue;

        dialogueController.StartDialogue("f67_device_001");
        SoundManager.Instance.PlayBGM(EBgm.Dialogue_67);
    }

    // 대사 종료
    public void OnDialogueFinished()
    {
        // 카드게임 직전 대사가 끝남
        if (state == FlowState.PreCardDialogue)
        {
            StartCardGame();
            return;
        }

        // 카드게임 직후 대사가 끝남
        if (state == FlowState.PostCardDialogue)
        {
            state = FlowState.ReadyToExit;

            if (endTrigger != null)
                endTrigger.SetActive(true);
        }
    }

    private void StartCardGame()
    {
        state = FlowState.CardGame;

        onCardGameStart?.Invoke();
    }

    // 카드게임 클리어
    public void OnCardGameCleared()
    {
        if (state != FlowState.CardGame)
            return;

        state = FlowState.PostCardDialogue;

        dialogueController.StartDialogue("f67_post_001");
    }


    public bool CanExit()
    {
        return state == FlowState.ReadyToExit;
    }


    private IEnumerator LoadNextFloorRoutine()
    {
        if (loadingCanvas != null)
            loadingCanvas.SetActive(true);

        float startTime = Time.unscaledTime;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(nextSceneName);

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
            yield return null;

        float elapsed = Time.unscaledTime - startTime;

        if (elapsed < minimumLoadingTime)
        {
            yield return new WaitForSecondsRealtime(
                minimumLoadingTime - elapsed
            );
        }

        operation.allowSceneActivation = true;
    }


    public void GoToNextFloor()
    {
        if (state != FlowState.ReadyToExit)
            return;

        StartCoroutine(LoadNextFloorRoutine());
    }
}