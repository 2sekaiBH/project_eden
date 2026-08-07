using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Floor399FlowController : MonoBehaviour
{
    private enum FlowState
    {
        Exploring,
        PreCardDialogue,
        CardGame,
        PostCardDialogue,
        Loading
    }

    [Header("Dialogue")]
    [SerializeField] private IntroDialogueController dialogueController;

    [Header("Card Game")]
    [SerializeField] private UnityEvent onCardGameStart;

    [Header("Loading")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private float minimumLoadingTime = 1.2f;

    [Header("Next Floor")]
    [SerializeField] private string nextSceneName = "04_Floor404Scene";

    private FlowState state = FlowState.Exploring;
    private bool isLoading = false;

    private void Start()
    {
        if (loadingCanvas != null)
            loadingCanvas.SetActive(false);
    }

    // 점프맵 맨 끝 Trigger에서 호출
    public void StartPreCardDialogue()
    {
        if (state != FlowState.Exploring)
            return;

        state = FlowState.PreCardDialogue;

        dialogueController.StartDialogue("f399_prebattle_001");
    }

    // DialogueSystem의 On Dialogue Finished에서 호출
    public void OnDialogueFinished()
    {
        // 카드게임 직전 대사 끝
        if (state == FlowState.PreCardDialogue)
        {
            state = FlowState.CardGame;
            onCardGameStart?.Invoke();
            return;
        }

        // 카드게임 직후의 모든 대사 끝
        if (state == FlowState.PostCardDialogue)
        {
            StartCoroutine(LoadNextFloorRoutine());
        }
    }

    // 카드게임 승리
    public void OnCardGameCleared()
    {
        if (state != FlowState.CardGame)
            return;

        state = FlowState.PostCardDialogue;

        dialogueController.StartDialogue("f399_post_001");
    }

    private IEnumerator LoadNextFloorRoutine()
    {
        if (isLoading)
            yield break;

        isLoading = true;
        state = FlowState.Loading;

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
}