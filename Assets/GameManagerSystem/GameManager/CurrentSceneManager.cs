using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    private CurrentSceneManager instance;
    public CurrentSceneManager Instance => instance;

    private string currentScene;
    public string CurrentScene => currentScene;
    private void OnEnable()
    {
        // 활성 씬이 변경될 때 호출되는 이벤트 등록
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene nextScene)
    {
        currentScene = nextScene.name;
        GameManager.Instance.SetCurrentScene(nextScene.name);
    }
}
