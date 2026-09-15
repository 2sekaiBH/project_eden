using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CardGameSceneBridge : MonoBehaviour
{
    [Header("Card Game Scene")]
    [SerializeField] private string cardGameSceneName = "Lahee_CardGame";

    [Header("Floor Objects")]
    [SerializeField] private GameObject floorPlayer;
    [SerializeField] private GameObject floorEventSystem;
    [SerializeField] private GameObject floorCamera;

    [Header("Result")]
    [SerializeField] private UnityEvent onGameCleared;
    [SerializeField] private UnityEvent onGameFailed;

    private CardGameManager cardGameManager;
    private bool isRunning;

    public void StartGame()
    {
        if (isRunning)
            return;

        StartCoroutine(LoadCardGame());
    }

    private IEnumerator LoadCardGame()
    {
        isRunning = true;

        // 기존 층의 조작/카메라/EventSystem 중지
        if (floorPlayer != null)
            floorPlayer.SetActive(false);

        if (floorEventSystem != null)
            floorEventSystem.SetActive(false);

        if (floorCamera != null)
            floorCamera.SetActive(false);

        AsyncOperation loadOperation =
            SceneManager.LoadSceneAsync(
                cardGameSceneName,
                LoadSceneMode.Additive
            );

        yield return loadOperation;

        Scene cardScene =
            SceneManager.GetSceneByName(cardGameSceneName);

        foreach (GameObject root in cardScene.GetRootGameObjects())
        {
            cardGameManager =
                root.GetComponentInChildren<CardGameManager>(true);

            if (cardGameManager != null)
                break;
        }

        if (cardGameManager == null)
        {
            Debug.LogError("CardGameManager를 찾지 못했습니다.");
            yield break;
        }

        cardGameManager.OnGameCleared.AddListener(HandleGameCleared);
        cardGameManager.OnGameFailed.AddListener(HandleGameFailed);

        cardGameManager.StartCardGame();
    }

    private void HandleGameCleared()
    {
        StartCoroutine(FinishGame(true));
    }

    private void HandleGameFailed()
    {
        StartCoroutine(FinishGame(false));
    }

    private IEnumerator FinishGame(bool cleared)
    {
        if (cardGameManager != null)
        {
            cardGameManager.OnGameCleared.RemoveListener(HandleGameCleared);
            cardGameManager.OnGameFailed.RemoveListener(HandleGameFailed);
        }

        AsyncOperation unloadOperation =
            SceneManager.UnloadSceneAsync(cardGameSceneName);

        yield return unloadOperation;

        // 다시 원래 층 복귀
        if (floorCamera != null)
            floorCamera.SetActive(true);

        if (floorEventSystem != null)
            floorEventSystem.SetActive(true);

        if (floorPlayer != null)
            floorPlayer.SetActive(true);

        isRunning = false;

        if (cleared)
            onGameCleared?.Invoke();
        else
            onGameFailed?.Invoke();
    }
}