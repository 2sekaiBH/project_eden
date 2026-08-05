using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        // 활성 씬이 변경될 때 호출되는 이벤트 등록
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene nextScene)
    {
        GameManager.Instance.SetCurrentScene(nextScene.name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!SceneManager.GetSceneByName("SettingsScene").isLoaded)
            {
                SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive);
            }
        }
    }
}
