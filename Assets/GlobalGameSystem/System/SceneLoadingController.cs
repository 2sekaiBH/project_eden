using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingController : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Animator loadingAnimator;

    [Header("Loading")]
    [SerializeField] private float minimumLoadingTime = 1.2f;

    private bool isLoading;

    public void LoadFloor67()
    {
        if (isLoading)
            return;

        StartCoroutine(LoadSceneRoutine("floor_67"));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        // 로딩 화면 켜기
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(true);
        }

        // 모래시계 애니메이션 처음부터 재생
        if (loadingAnimator != null)
        {
            loadingAnimator.Play(0, 0, 0f);
        }

        float startTime = Time.unscaledTime;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        // Scene 로딩 대기
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 로딩 화면이 너무 빨리 사라지지 않도록 최소 시간 보장
        float elapsed =
            Time.unscaledTime - startTime;

        if (elapsed < minimumLoadingTime)
        {
            yield return new WaitForSecondsRealtime(
                minimumLoadingTime - elapsed
            );
        }

        operation.allowSceneActivation = true;
    }
}