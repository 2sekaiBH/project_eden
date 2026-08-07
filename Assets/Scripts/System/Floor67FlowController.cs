using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor67FlowController : MonoBehaviour
{
    private enum FlowState
    {
        Exploring,
        PostGameDialogue,
        ReadyToExit
    }

    [Header("Dialogue")]
    [SerializeField] private IntroDialogueController dialogueController;

    [Header("Exit")]
    [SerializeField] private GameObject endTrigger;

    [Header("Loading")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Animator loadingAnimator;
    [SerializeField] private float minimumLoadingTime = 1.2f;

    [Header("Next Floor")]
    [SerializeField] private string nextSceneName = "floor_213";

    private FlowState state = FlowState.Exploring;
    private bool isLoading = false;

    private void Start()
    {
        if (endTrigger != null)
            endTrigger.SetActive(false);

        if (loadingCanvas != null)
            loadingCanvas.SetActive(false);
    }

    // 게임 성공
    public void OnGameCleared()
    {
        state = FlowState.PostGameDialogue;

        dialogueController.StartDialogue("f67_post_001");
    }

    // 후속 대사 종료
    public void OnDialogueFinished()
    {
        if (state != FlowState.PostGameDialogue)
            return;

        state = FlowState.ReadyToExit;

        if (endTrigger != null)
            endTrigger.SetActive(true);
    }

    // 맵 끝에 도착
    public void GoToNextFloor()
    {
        if (state != FlowState.ReadyToExit)
            return;

        if (isLoading)
            return;

        StartCoroutine(LoadNextFloorRoutine());
    }

    private IEnumerator LoadNextFloorRoutine()
    {
        isLoading = true;

        // 로딩 화면 ON
        if (loadingCanvas != null)
            loadingCanvas.SetActive(true);

        // 모래시계 애니메이션 처음부터 재생
        if (loadingAnimator != null)
            loadingAnimator.Play(0, 0, 0f);

        float startTime = Time.unscaledTime;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(nextSceneName);

        operation.allowSceneActivation = false;

        // 실제 씬 로딩 완료까지 대기
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 로딩 화면이 너무 찰나에 지나가지 않도록
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